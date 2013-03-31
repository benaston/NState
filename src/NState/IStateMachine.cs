using System.Collections.Generic;

namespace NState
{
    public interface IStateMachine<TState, TTransitionActionStatus> where TState : State
    {
        string Name { get; set; }

        IEnumerable<IStateTransition<TState, TTransitionActionStatus>> StateTransitions { get; }

        TState InitialState { get; set; }

        IStateMachine<TState, TTransitionActionStatus> Parent { get; set; }

        /// <summary>
        /// Key is state machine name.
        /// </summary>
        Dictionary<string, IStateMachine<TState, TTransitionActionStatus>> Children { get; set; }

        TState CurrentState { get; set; }

        /// <summary>
        /// Kept as a method on the interface (rather than extension method) to permit easier extension.
        /// </summary>
        TTransitionActionStatus TriggerTransition(TState targetState, dynamic statefulObject, dynamic dto = default(dynamic));
    }
}