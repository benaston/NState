namespace NState
{
    using System;

    public abstract class StateTransition<TStatefulDomainObject, TState> :
        IStateTransition
            <TStatefulDomainObject, TState>
        where TStatefulDomainObject :
            IStateful<TStatefulDomainObject, TState>
        where TState : State
    {
        private readonly TStatefulDomainObject _statefulDomainObject;

        protected StateTransition(
            Func<TStatefulDomainObject, TState, dynamic, TStatefulDomainObject> transitionFunction = null)
        {
            TransitionFunction = transitionFunction ?? ((o, s, args) => o); //identity
        }

        public Func<TStatefulDomainObject, TState, dynamic, TStatefulDomainObject> TransitionFunction { get; private set; }

        public abstract TState[] StartState { get; }

        public abstract TState[] EndState { get; }

        public Func<TStatefulDomainObject, TState, dynamic, TStatefulDomainObject> Transition
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