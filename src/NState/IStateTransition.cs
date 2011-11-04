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
        TState[] StartStates { get; }

        TState[] EndStates { get; }

        //
        //Func<TStatefulObject, TState, dynamic,TStatefulObject> TransitionFunction { get; }

        Action<TState, dynamic> TransitionFunction { get; }

        //Func<dynamic, dynamic, dynamic, dynamic> TransitionFunction { get; }
    }
}