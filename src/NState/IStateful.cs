namespace NState
{
    /// <summary>
    ///   Responsible for defining the interface for types 
    ///   that may be used inside with the StateMachine type 
    ///   (akin to workflow.)
    /// </summary>
    public interface IStateful<TStatefulObject, TState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TStateMachineTypeEnum>
        where TState : State
        where TStateMachineTypeEnum : struct
    {
        IStateMachine<TStatefulObject, TState, TStateMachineTypeEnum> StateMachine { get; set; }

        TState CurrentState { get; }

        TStatefulObject PerformTransition(TState targetState);
    }
}