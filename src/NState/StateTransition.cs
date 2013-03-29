using System;

namespace NState
{
    public abstract class StateTransition<TState, TTransitionActionStatus> :
        IStateTransition<TState, TTransitionActionStatus>
        where TState : State
    {
        protected StateTransition(Func<TState, dynamic, dynamic, bool> condition = null,
                                  TransitionAction<TState, TTransitionActionStatus> transitionAction = null)
        {
            Condition = condition ?? ((s, statefulObject, dto) => true);
            TransitionAction = transitionAction ?? new NullTransitionAction<TState, TTransitionActionStatus>();
        }

        public abstract TState[] StartStates { get; }

        public abstract TState[] EndStates { get; }

        public TransitionAction<TState, TTransitionActionStatus> TransitionAction { get; private set; }

        public Func<TState, dynamic, dynamic, bool> Condition { get; private set; }
    }
}