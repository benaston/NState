namespace NState
{
    using System;

    public abstract class StateTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> :
        IStateTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulDomainObject : IStateful<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        public Func<TStatefulDomainObject, TState, TStatefulDomainObject> TransitionFunction { get; set; }

        public abstract TState StartState { get; }

        public abstract TState EndState { get; }

        public Func<TStatefulDomainObject, TState, TStatefulDomainObject> Transition
        {
            get
            {
                return (o, s) =>
                           {
                               OnRaiseBeforeTransition();
                               var v = TransitionFunction(o, s);
                               OnRaiseBeforeTransition();
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