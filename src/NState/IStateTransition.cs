using System;

namespace NState
{
    public interface IStateTransition<TState, TTransitionActionStatus> where TState : State
    {
        TState[] StartStates { get; }

        TState[] EndStates { get; }

        /// <summary>
        /// Logic to be run when the transition occurs.
        /// </summary>
        TransitionAction<TState, TTransitionActionStatus> TransitionAction { get; }

        /// <summary>
        /// A constraint which will permit the transition only 
        /// when it evaluates to true.
        /// Arg1 is the state being transitioned to.
        /// Arg2 is the stateful object undergoing the transition.
        /// Arg3 is a DTO containing any additional data required to perform the transition.
        /// </summary>
        Func<TState, dynamic, dynamic, bool> Condition { get; }
    }
}