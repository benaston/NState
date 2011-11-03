namespace NState
{
    /// <summary>
    ///   Responsible for defining the interface for types 
    ///   that may be used inside with the StateMachine type 
    ///   (akin to workflow.)
    /// </summary>
    public interface IStateful<TStatefulObject, TState, TBaseState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseState, TStateMachineTypeEnum>
        where TState : State
        //where TBaseDomainObject :
        //    IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        ///// <summary>
        ///// Given a stateful domain object retrieves the state machine relevant to it
        ///// from the parent state composite object.
        ///// </summary>
        //IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> 
        //    GetStateMachine(IStateMachine<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> stateMachine);

        IStateMachine<TStatefulObject, TState, TBaseState, TStateMachineTypeEnum> StateMachine { get; set; }

        TState CurrentState { get; }

        TStatefulObject PerformTransition(TState targetState);
    }
}