namespace NState
{
    using System.Dynamic;

    /// <summary>
    ///   Responsible for defining the interface for types
    ///   that may be used inside with the StateMachine type
    ///   (akin to workflow.)
    /// </summary>
    public interface IStateful<TStatefulObject, TState>
        where TStatefulObject : Stateful<TStatefulObject, TState>
        where TState : State
    {
        IStateMachine<TState> StateMachine { get; set; }

        TState CurrentState { get; }

        TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject, TState targetState,
                                                           ExpandoObject dto);
    }
}