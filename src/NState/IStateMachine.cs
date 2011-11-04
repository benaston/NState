namespace NState
{
    using System;
    using System.Collections.Generic;

    public interface IStateMachine {}

    /// <summary>
    ///   Responsible for defining the interface for types that
    ///   control the transitions between state machine states.
    /// </summary>
    public interface IStateMachine<TStatefulObject, TState> : IStateMachine where TStatefulObject : Stateful<TStatefulObject, TState>
        where TState : State
    {
        IEnumerable<IStateTransition<TState>> StateTransitions { get; }

        TState StartState { get; set; }

        IStateMachine<TStatefulObject, TState> ParentStateMachine { get; set; }

        TState CurrentState { get; set; }

        Dictionary <DateTime, IStateTransition<TState>> History { get; set; }

        TStateful TriggerTransition<TStateful>(TStateful opportunity, TState targetState, dynamic dto);
    }
}