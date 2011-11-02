namespace NState
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using NBasicExtensionMethod;
    using NSure;
    using ArgumentNullException = NHelpfulException.FrameworkExceptions.ArgumentNullException;

    ////  which, if not present, is assumed to be the current state machine.
    /// <summary>
    ///   TODO: BA; basedomainobject might be modified to be a parent statemachine
    ///   TODO: BA add persistence of current state and subsequent retrieval.
    ///   Responsible for defining the base functionality for object state machines
    ///   to enable specification of valid state changes to be applied to object
    ///   instances.
    ///   NOTE 1: BA; http://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum
    /// </summary>
    /// <typeparam name="TBaseDomainObject">This is the root of the tree of domain objects in the state machine?</typeparam>
    public class StateMachine<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> :
        IStateMachine<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulDomainObject : IStateful<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        protected readonly IEnumerable<IStateTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>>
            StateTransitions;

        public StateMachine(
            IEnumerable<IStateTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>> stateTransitions,
            TState startState,
            Dictionary<TStateMachineTypeEnum, IStateMachine> childStateMachines = null)
        {
            Ensure.That(stateTransitions.IsNotNull(), "stateTransitions not supplied.");

            StateTransitions = stateTransitions;
            ChildStateMachines = childStateMachines ?? new Dictionary<TStateMachineTypeEnum, IStateMachine>();
            StartState = startState;
            CurrentState = startState;
        }

        public TState StartState { get; set; }

        public TState CurrentState { get; set; }

        public Dictionary<DateTime, IStateTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>>
            History
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///   Select transition to invoke, and invoke.
        ///   NOTE: BA; many to many state relationships possible. 
        ///   Performs first matching transition that is found.
        ///   NOTE: BA; transitions to self must be explicitly defined.
        /// </summary>
        public TStatefulDomainObject PerformTransition(TStatefulDomainObject statefulDomainObject, TState targetState)
        {
            Ensure.That<ArgumentNullException>(statefulDomainObject.IsNotNull(),
                                               "statefulDomainObject not supplied.");

            OnRaiseBeforeEveryTransition();

            try
            {
                //modify to if the state itself has a transition or if any of the enclosing state-composite has a state transition
                //then follow it
                //move current state onto the domain object itself
                var v = StateTransitions.First(t => t.StartState == CurrentState && t.EndState == targetState).
                    Transition
                    (statefulDomainObject, targetState);
                CurrentState = targetState;
                //statefulDomainObject.CurrentState = targetState;
                OnRaiseAfterEveryTransition();

                return v;
            }
            catch (Exception e)
            {
                var i = new InvalidStateTransitionException<TState>(CurrentState, targetState);
                //i.Log();

                throw i;
            }
        }

        public Dictionary<TStateMachineTypeEnum, IStateMachine> ChildStateMachines { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public event EventHandler RaiseBeforeEveryTransitionEvent;

        public event EventHandler RaiseAfterEveryTransitionEvent;

        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        protected virtual void OnRaiseBeforeEveryTransition()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            var handler = RaiseBeforeEveryTransitionEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                //e.Message += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, new EventArgs());
            }
        }

        protected virtual void OnRaiseAfterEveryTransition()
        {
            var handler = RaiseAfterEveryTransitionEvent;

            if (handler != null) // Event will be null if there are no subscribers
            {
                handler(this, new EventArgs());
            }
        }

        //use for state machine persistence and re-creation
    }
}