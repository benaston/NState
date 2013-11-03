using System;
using System.Collections.Generic;
using System.Linq;

namespace NState
{
    public class StateMachine<TState, TTransitionActionStatus> : IStateMachine<TState, TTransitionActionStatus> 
        where TState : State
    {
        private Dictionary<string, IStateMachine<TState, TTransitionActionStatus>> _children =
            new Dictionary<string, IStateMachine<TState, TTransitionActionStatus>>();

        public StateMachine() { } //for deserialization

        public StateMachine(string name,
                            IEnumerable<IStateTransition<TState, TTransitionActionStatus>> stateTransitions,
                            TState initialState,
                            TState finalState = null,
                            IStateMachine<TState, TTransitionActionStatus> parentStateMachine = null,
                            bool permitSelfTransition = true,
                            bool bypassTransitionBehaviorForSelfTransition = false) //if permitted
        {
            if (name == null) { throw new ArgumentNullException("name"); }
            if (stateTransitions == null) { throw new ArgumentNullException("stateTransitions"); }
            if (initialState == null) { throw new ArgumentNullException("initialState"); }

            Name = name;
            StateTransitions = stateTransitions.ToArray();
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

        public string Name { get; set; }

        public IStateTransition<TState, TTransitionActionStatus>[] StateTransitions { get; protected set; }

        public TState InitialState { get; set; }

        public TState FinalState { get; set; }

        public IStateMachine<TState, TTransitionActionStatus> Parent { get; set; }

        public Dictionary<string, IStateMachine<TState, TTransitionActionStatus>> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public TState CurrentState { get; set; }

        public virtual TTransitionActionStatus TriggerTransition(TState targetState,
                                                                 dynamic statefulObject,
                                                                 dynamic args = default(dynamic))
        {
            return StateMachineTransitionHelper.TriggerTransition(this, targetState, statefulObject, args);
        }

        public bool PermitSelfTransition { get; set; }

        public bool BypassTransitionBehaviorForSelfTransition { get; set; }
    }
}