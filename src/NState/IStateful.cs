namespace NState
{
    /// <summary>
    ///   Responsible for defining the interface for types 
    ///   that may be used inside with the StateMachine type 
    ///   (akin to workflow.)
    /// </summary>
    public interface IStateful<TStatefulObject, TState>
        where TStatefulObject : Stateful<TStatefulObject, TState>
        where TState : State
    {
        IStateMachine<TStatefulObject, TState> StateMachine { get; set; }

        TState CurrentState { get; }

        TStateful TriggerTransition<TStateful>(TState targetState, dynamic dto);
    }
}