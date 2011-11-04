namespace NState
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        where TStatefulObject : Stateful<TStatefulObject, TState>
        where TState : State
    {
        /// <summary>
        ///   Required for deserialization.
        /// </summary>
        public StateMachine() {}

        public StateMachine(
            IEnumerable<IStateTransition<TState>> stateTransitions,
            TState startState,
            IStateMachine<TStatefulObject, TState> parentStateMachine = null)
        {
            Ensure.That(stateTransitions.IsNotNull(), "stateTransitions not supplied.");

            StateTransitions = stateTransitions;
            StartState = startState;
            //ParentStateMachine = parentStateMachine;
            CurrentState = startState;
        }

        public IEnumerable<IStateTransition<TState>> StateTransitions { get; protected set; }

        public TState StartState { get; set; }

        //[JsonProperty(TypeNameHandling = TypeNameHandling.Objects)]
        //public IStateMachine<TStatefulObject, TState> ParentStateMachine { get; set; }

        public TState CurrentState { get; set; }

        public Dictionary<DateTime, IStateTransition<TState>> History { get; set; }

        public TStateful PerformTransition<TStateful>(TStateful statefulObject, TState targetState,
                                                      dynamic dto = default(dynamic))
        {
            Ensure.That<ArgumentNullException>(statefulObject.IsNotNull(),
                                               "statefulObject not supplied.");

            try
            {
                if (CurrentState != targetState) //make this explicit?
                {
                    var matches = StateTransitions.Where(t =>
                                                             t.StartStates.Where(s => s == CurrentState).Any() &&
                                                             t.EndStates.Where(e => e == targetState).Any());
                    if (matches.Any())
                    {
                        OnRaiseBeforeEveryTransition();
                        matches.First().TransitionFunction(targetState, dto);
                        CurrentState = targetState;
                        OnRaiseAfterEveryTransition();
                    }
                    else
                    {
                        //if (ParentStateMachine == null)
                        //{
                            throw new Exception(); //to be caught below, refactor
                        //}

                        //ParentStateMachine.PerformTransition<TStateful>(statefulObject, targetState, dto);
                    }
                }

                return statefulObject;
            }
            catch (Exception e)
            {
                throw new InvalidStateTransitionException<TState>(CurrentState, targetState, innerException: e);
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

//state machines sharing common transitions need to be part of the same inheritance hierarchy
//var localTransitions = matchingTransitionsFunction(StateTransitions);

//possibly removal of type return constraint would enable covariance?
//i think we can say this - each state machine can hold a reference to a parent 
//state machine, *which is typed in terms of state to the former*
//may need to refactor the code so the finding of the matching transition function 
//occurs before the actual "transitionto behavior"?

//statefulObject = localTransitions.Any() ? localTransitions.First()
//    .TransitionFunction(statefulObject, targetState, dto) : ParentStateMachines.Where(sm => sm)


/*pseudocode: test for matching local transitions, then invoke against parent (which should invoke its parent)*/
/*may need to separat eout the transition function onto the IStateful interface (and remove some of the params like the stateful object
.Could remove the stateful object and if needed, have it passed in via the dto, but that kind of sucks.*/
/*perhaps modify the concept of the transition function to not affect the domain object? - investigate by constructing a test with this behavior*/


//first  extract the recursive alo from this and start using interfaces/base types instead of concrete states