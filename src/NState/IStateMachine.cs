using System.Collections.Generic;

namespace NState
{
    public interface IStateMachine {}

    /// <summary>
    /// Responsible for defining the interface for types 
    /// that control the transitions between state machine states.
    /// </summary>
    public interface IStateMachine<TState, TTransitionActionStatus> : IStateMachine where TState : State
    {
        string Name { get; set; }

        IEnumerable<IStateTransition<TState, TTransitionActionStatus>> StateTransitions { get; }

        TState InitialState { get; set; }

        IStateMachine<TState, TTransitionActionStatus> Parent { get; set; }

        /// <summary>
        /// Key is SM name.
        /// </summary>
        Dictionary<string, IStateMachine<TState, TTransitionActionStatus>> Children { get; set; }

        TState CurrentState { get; set; }

        /// <summary>
        /// Kept as a method on the interface (rather than extension method) to permit easier extension.
        /// </summary>
        TTransitionActionStatus TriggerTransition(TState targetState, dynamic statefulObject, dynamic dto = default(dynamic));
    }
}