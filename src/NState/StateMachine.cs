using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Transactions;
using NState.Exceptions;
using Newtonsoft.Json;

namespace NState
{
    /// <summary>
    /// Enables specification of valid state changes to be applied to object instances.
    /// </summary>
    public class StateMachine<TState, TTransitionStatus> : IStateMachine<TState, TTransitionStatus> where TState : State
    {
        private Dictionary<string, IStateMachine<TState, TTransitionStatus>> _children =
            new Dictionary<string, IStateMachine<TState, TTransitionStatus>>();

        public StateMachine() { } //for deserialization

        public StateMachine(string name,
                            IEnumerable<IStateTransition<TState, TTransitionStatus>> stateTransitions,
                            TState initialState,
                            TState finalState = null,
                            IStateMachine<TState, TTransitionStatus> parentStateMachine = null,
                            bool permitSelfTransition = true,
                            bool bypassTransitionBehaviorForSelfTransition = false) //if permitted
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (stateTransitions == null)
            {
                throw new ArgumentNullException("stateTransitions");
            }

            if (initialState == null)
            {
                throw new ArgumentNullException("initialState");
            }

            Name = name;
            StateTransitions = stateTransitions;
            InitialState = initialState;
            FinalState = finalState;
            Parent = parentStateMachine;
            PermitSelfTransition = permitSelfTransition;
            BypassTransitionBehaviorForSelfTransition = bypassTransitionBehaviorForSelfTransition;

            if (parentStateMachine != null)
            {
                Parent.Children.Add(Name, this);
            }

            CurrentState = initialState;
        }

        public TState FinalState { get; set; }

        public bool PermitSelfTransition { get; set; }

        public bool BypassTransitionBehaviorForSelfTransition { get; set; }

        public string Name { get; set; }

        public IEnumerable<IStateTransition<TState, TTransitionStatus>> StateTransitions { get; protected set; }

        public TState InitialState { get; set; }

        public IStateMachine<TState, TTransitionStatus> Parent { get; set; }

        public Dictionary<string, IStateMachine<TState, TTransitionStatus>> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public TState CurrentState { get; set; }

        public Dictionary<DateTime, IStateTransition<TState, TTransitionStatus>> History { get; set; }

        /// <summary>
        /// NOTE 1: http://cs.hubfs.net/blogs/hell_is_other_languages/archive/2008/01/16/4565.aspx.
        /// NOTE 2: this could be in-memory transactionalised using the 
        /// memento pattern, or information could be sent to F# (see NOTE 1).
        /// </summary>
        public virtual TTransitionStatus TriggerTransition(TState targetState,
                                              dynamic statefulObject,
                                              dynamic args = default(dynamic))
        {
            var result = default(TTransitionStatus);
            if (targetState == null)
            {
                throw new ArgumentNullException("targetState");
            }
            
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (CurrentState == targetState && !PermitSelfTransition)
            {
                throw new SelfTransitionException();
            }

            if (CurrentState == FinalState)
            {
                throw new FinalStateTransitionException();
            }

            if ((CurrentState != targetState ||
                (CurrentState == targetState && !BypassTransitionBehaviorForSelfTransition)))
            {
                var matches = StateTransitions.Where(t => t.StartStates.Any(s => s == CurrentState) &&
                                                            t.EndStates.Any(e => e == targetState)).ToList();
                var match = matches.Count > 0 ? matches[0] : null;
                if (match != null && match.Condition(targetState, statefulObject, args))
                {
                    //see note 2
                    {
                        OnRaiseBeforeEveryTransition();
                        CurrentState.ExitAction(args);
                        result = match.TransitionAction.Run(targetState, this, statefulObject, args);
                        targetState.EntryAction(args);
                        CurrentState = targetState;
                        OnRaiseAfterEveryTransition();
                    }
                }
                else
                {
                    if (Parent == null)
                    {
                        throw new InvalidStateTransitionException<TState>(CurrentState,
                                                                targetState);
                    }

                    Parent.TriggerTransition(targetState, args);
                }
            }

            return result;
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
            EventHandler handler = RaiseBeforeEveryTransitionEvent;

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
            EventHandler handler = RaiseAfterEveryTransitionEvent;

            if (handler != null) // Event will be null if there are no subscribers
            {
                handler(this, new EventArgs());
            }
        }

        public string ToJson()
        {
            dynamic dto = StateMachineSerializationHelper.SerializeToDto(this, new ExpandoObject());
            dynamic s = JsonConvert.SerializeObject(dto,
                                                    Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        ObjectCreationHandling =
                                                            ObjectCreationHandling.Replace
                                                    });

            return s;
        }

        /// <summary>
        /// Not in constructor because SM tree may not be completely initialized by constructor in current implementation.
        /// </summary>
        public IStateMachine<TState, TTransitionStatus> InitializeFromJson(string json)
        {
            return StateMachineSerializationHelper.InitializeWithDto(this,
                                                                     JsonConvert.DeserializeObject
                                                                         (json));
        }
    }
}