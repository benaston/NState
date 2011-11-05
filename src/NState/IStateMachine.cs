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
        string Name { get; set; }

        IEnumerable<IStateTransition<TState>> StateTransitions { get; }

        TState StartState { get; set; }

        IStateMachine<TStatefulObject, TState> Parent { get; set; }

        /// <summary>
        /// Key is SM name.
        /// </summary>
        Dictionary<string, IStateMachine<TStatefulObject, TState>> Children { get; set; }

        TState CurrentState { get; set; }

        Dictionary <DateTime, IStateTransition<TState>> History { get; set; }

        void TriggerTransition(TState targetState, dynamic dto);
    }
}