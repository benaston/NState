namespace NState
{
    using System;

    public abstract class StateTransition<TState> :
        IStateTransition<TState>
        where TState : State
    {
        protected StateTransition(
            Action<TState, IStateMachine<TState>, dynamic> transitionAction = null,
            Func<TState, dynamic, bool> condition = null)
        {
            Condition = condition ?? ((s, args) => true);
            TransitionAction = transitionAction ?? ((s, sm, args) => { });
        }

        public Func<TState, dynamic, bool> Condition { get; private set; }

        public Action<TState, IStateMachine<TState>, dynamic> TransitionAction { get; private set; }

        public abstract TState[] InitialStates { get; }

        public abstract TState[] EndStates { get; }
    }
}