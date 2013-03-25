using System;

namespace NState
{
    /// <summary>
    /// Defines the interface for a state transition for 
    /// a type that supports "workflow".
    /// </summary>
    public interface IStateTransition<TState, TTransitionStatus> where TState : State
    {
        TState[] InitialStates { get; }

        TState[] EndStates { get; }

        /// <summary>
        /// Logic to be run when the transition occurs.
        /// </summary>
        TransitionAction<TState, TTransitionStatus> TransitionAction { get; }

        /// <summary>
        /// A constraint which will permit the transition only 
        /// when it evaluates to true after the trigger occurs.
        /// Arg1 is the target state.
        /// Arg2 is the stateful object.
        /// Arg3 is a DTO object.
        /// </summary>
        Func<TState, dynamic, dynamic, bool> Condition { get; }
    }
}