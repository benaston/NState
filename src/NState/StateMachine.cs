namespace NState
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MoreLinq;
    using NBasicExtensionMethod;
    using Newtonsoft.Json;
    using NSure;
    using ArgumentNullException = NHelpfulException.FrameworkExceptions.ArgumentNullException;

    /// <summary>
    ///   Enables specification of valid state changes to be applied to object
    ///   instances.
    /// </summary>
    [Serializable]
    public class StateMachine<TStatefulObject, TState> :
        IStateMachine<TStatefulObject, TState>
        where TStatefulObject : IStateful<TStatefulObject, TState>
        where TState : State
    {
        protected readonly
            IEnumerable
                <IStateTransition<TStatefulObject, TState>>
            StateTransitions;

        public StateMachine(
            IEnumerable
                <IStateTransition<TStatefulObject, TState>>
                stateTransitions,
            TState startState,
            List<IStateMachine> childStateMachines = null,
            List<IStateMachine> parentStateMachines = null)
        {
            Ensure.That(stateTransitions.IsNotNull(), "stateTransitions not supplied.");

            StateTransitions = stateTransitions;
            ChildStateMachines = childStateMachines ?? new List<IStateMachine>();
            ParentStateMachines = parentStateMachines ?? new List<IStateMachine>();
            StartState = startState;
            CurrentState = startState;
        }

        public TState StartState { get; set; }

        public TState CurrentState { get; set; }

        [JsonProperty(TypeNameHandling = TypeNameHandling.Objects)]
        public List<IStateMachine> ChildStateMachines { get; set; }

        [JsonProperty(TypeNameHandling = TypeNameHandling.Objects)]
        public List<IStateMachine> ParentStateMachines { get; set; }

        public Dictionary<DateTime, IStateTransition<TStatefulObject, TState>> History { get; set; }

        /// <summary>
        ///   WIP - hierarchy!
        /// </summary>
        public TStatefulObject TransitionTo(TStatefulObject statefulObject, TState targetState,
                                                       dynamic dto = default(dynamic))
        {
            Ensure.That<ArgumentNullException>(statefulObject.IsNotNull(),
                                               "statefulObject not supplied.");

            OnRaiseBeforeEveryTransition();

            try
            {
                if (CurrentState != targetState) //make this explicit?
                {
                    var transition = StateTransitions.Where(
                       t =>
                       t.StartState.Where(s => s == CurrentState).Any() &&
                       t.EndState.Where(e => e == targetState).Any()).First().Transition;

                    statefulObject = transition(statefulObject, targetState, dto);

                    CurrentState = targetState;
                }
                OnRaiseAfterEveryTransition();

                return statefulObject;
            }
            catch (Exception)
            {
                var i = new InvalidStateTransitionException<TState>(CurrentState, targetState);

                throw i;
            }
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
    }
}