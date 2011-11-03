namespace NState
{
    using System;

    public abstract class StateTransition<TStatefulObject, TState> :
        IStateTransition
            <TStatefulObject, TState>
        where TStatefulObject :
            IStateful<TStatefulObject, TState>
        where TState : State
    {
        protected StateTransition(
            Func<TStatefulObject, TState, dynamic, TStatefulObject> transitionFunction = null)
        {
            TransitionFunction = transitionFunction ?? ((o, s, args) => o); //identity
        }

        public Func<TStatefulObject, TState, dynamic, TStatefulObject> TransitionFunction { get; private set; }

        public abstract TState[] StartStates { get; }

        public abstract TState[] EndStates { get; }

        public Func<TStatefulObject, TState, dynamic, TStatefulObject> TransitionTo
        {
            get
            {
                return (o, s, dto) =>
                           {
                               OnRaiseBeforeTransition();
                               var v = TransitionFunction(o, s, dto);
                               OnRaiseAfterTransition();
                               return v;
                           };
            }
        }

        public event EventHandler RaiseBeforeTransitionEvent;

        public event EventHandler RaiseAfterTransitionEvent;

        protected virtual void OnRaiseBeforeTransition()
        {
            var handler = RaiseBeforeTransitionEvent;

            if (handler != null) // Event will be null if there are no subscribers
            {
                handler(this, new EventArgs());
            }
        }

        protected virtual void OnRaiseAfterTransition()
        {
            var handler = RaiseAfterTransitionEvent;

            if (handler != null) // Event will be null if there are no subscribers
            {
                handler(this, new EventArgs());
            }
        }
    }
}