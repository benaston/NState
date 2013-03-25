using System;
using System.Collections.Generic;

namespace NState
{
    public interface IStateMachine {}

    /// <summary>
    /// Responsible for defining the interface for types 
    /// that control the transitions between state machine states.
    /// </summary>
    public interface IStateMachine<TState, TTransitionStatus> : IStateMachine where TState : State
    {
        string Name { get; set; }

        IEnumerable<IStateTransition<TState, TTransitionStatus>> StateTransitions { get; }

        TState InitialState { get; set; }

        IStateMachine<TState, TTransitionStatus> Parent { get; set; }

        /// <summary>
        /// Key is SM name.
        /// </summary>
        Dictionary<string, IStateMachine<TState, TTransitionStatus>> Children { get; set; }

        TState CurrentState { get; set; }

        Dictionary<DateTime, IStateTransition<TState, TTransitionStatus>> History { get; set; }

        TTransitionStatus TriggerTransition(TState targetState, dynamic statefulObject, dynamic dto = default(dynamic));
    }
}