namespace NState
{
    using System;
    using NSure;

    public abstract class StateTransition<TStatefulDomainObject, TState, TBaseState,
                                          TStateMachineTypeEnum> :
                                              IStateTransition
                                                  <TStatefulDomainObject, TState, TBaseState,
                                                  TStateMachineTypeEnum>
        where TStatefulDomainObject :
            IStateful<TStatefulDomainObject, TState, TBaseState, TStateMachineTypeEnum>
        where TState : State
        //where TBaseDomainObject :
        //    IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        private readonly TStatefulDomainObject _statefulDomainObject;

        protected StateTransition(Func<TStatefulDomainObject, TState, TStatefulDomainObject> transitionFunction)
        {
            Ensure.That(transitionFunction != null, "transitionFunction not supplied");

            TransitionFunction = transitionFunction;
        }

        public Func<TStatefulDomainObject, TState, TStatefulDomainObject> TransitionFunction { get; private set; }

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