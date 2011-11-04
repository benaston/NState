namespace NState
{
    using System;

    public abstract class StateTransition<TStatefulObject, TState> :
        IStateTransition<TState>
        //where TStatefulObject :
        //    Stateful<TStatefulObject, TState>
        where TState : State
    {
        //protected StateTransition(
        //    Func<TStatefulObject, TState, dynamic, TStatefulObject> transitionFunction = null)
        //{
        //    TransitionFunction = transitionFunction ?? ((o, s, args) => o); //identity
        //}

        protected StateTransition(
            Action<TState, dynamic> transitionFunction = null)
        {
            TransitionFunction = transitionFunction ?? ((s, args) => { });
        }

        public Action<TState, dynamic> TransitionFunction { get; private set; }

        //public Func<TStatefulObject, TState, dynamic, TStatefulObject> TransitionFunction { get; private set; }
        
        //public Func<dynamic, dynamic, dynamic, dynamic> TransitionFunction { get; private set; }

        public abstract TState[] StartStates { get; }

        public abstract TState[] EndStates { get; }

        public Func<TStatefulObject, TState, dynamic, TStatefulObject> TransitionTo
        {
            get
            {
                return (o, s, args) =>
                           {
                               OnRaiseBeforeTransition();
                               var v = TransitionFunction(s, args);
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