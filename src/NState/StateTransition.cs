using System;

namespace NState
{
    public abstract class StateTransition<TState, TTransitionStatus> :
        IStateTransition<TState, TTransitionStatus>
        where TState : State
    {
        protected StateTransition(Func<TState, dynamic, dynamic, bool> condition = null,
                                  TransitionAction<TState, TTransitionStatus> transitionAction = null)
        {
            Condition = condition ?? ((s, statefulObject, dto) => true);
            TransitionAction = transitionAction ?? new NullTransitionAction<TState, TTransitionStatus>();
        }

        public abstract TState[] StartStates { get; }

        public abstract TState[] EndStates { get; }

        public TransitionAction<TState, TTransitionStatus> TransitionAction { get; private set; }

        public Func<TState, dynamic, dynamic, bool> Condition { get; private set; }
    }
}