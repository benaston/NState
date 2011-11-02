namespace NState
{
    /// <summary>
    ///   Responsible for defining the interface for types 
    ///   that may be used inside with the StateMachine type 
    ///   (akin to workflow.)
    /// </summary>
    public interface IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> 
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> 
            GetStateMachineFromRootComposite(IStateMachine<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> stateMachine);
    }
}