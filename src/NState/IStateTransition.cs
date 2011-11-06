namespace NState
{
    using System;

    /// <summary>
    ///   Defines the interface for a state transition for a type that supports
    ///   "workflow". Examples of actions being performed during a transition
    ///   include the sending of an email or SMS, the persisting of information 
    ///   to the database, or an onscreen notification or the billing of funds.
    ///   Note that a computer program might invoke the state transition.
    /// </summary>
    public interface IStateTransition<TState>
        where TState : State
    {
        TState[] InitialStates { get; }

        TState[] EndStates { get; }

        /// <summary>
        /// A constraint which will fire the transition only when it is evaluated to true after the trigger occurs.
        /// </summary>
        Func<TState, dynamic, bool> Condition { get; }

        /// <summary>
        /// An action which is executed when performing a certain transition.
        /// </summary>
        Action<TState, IStateMachine<TState>, dynamic> TransitionAction { get; }
    }
}