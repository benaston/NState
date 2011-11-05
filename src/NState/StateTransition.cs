namespace NState
{
    using System;

    public abstract class StateTransition<TState> :
        IStateTransition<TState>
        where TState : State
    {
        protected StateTransition(
            Action<TState, IStateMachine<TState>, dynamic> transitionFunction = null, Func<TState, dynamic, bool> guard = null)
        {
            Guard = guard ?? ((s, args) => true);
            TransitionFunction = transitionFunction ?? ((s, sm, args) => { });
        }

        public Func<TState, dynamic, bool> Guard { get; private set; }

        public Action<TState, IStateMachine<TState>, dynamic> TransitionFunction { get; private set; }

        public abstract TState[] StartStates { get; }

        public abstract TState[] EndStates { get; }
    }
}