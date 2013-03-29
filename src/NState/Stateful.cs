using System;
using System.Dynamic;
using NState.Exceptions;

namespace NState
{
    /// <summary>
    /// Inherit from this if you want to make your type stateful.
    /// </summary>
    public abstract class Stateful<TState, TTransitionActionStatus> : IStateful<TState, TTransitionActionStatus> where TState : State
    {
        protected Stateful(IStateMachine<TState, TTransitionActionStatus> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public TState ParentState
        {
            get
            {
                if (StateMachine.Parent == null)
                {
                    throw new ParentStateNotAvailableException();
                }

                return StateMachine.Parent.CurrentState;
            }
        }

        public IStateMachine<TState, TTransitionActionStatus> StateMachine { get; set; }

        public TState CurrentState
        {
            get { return StateMachine.CurrentState; }
        }

        public TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject,
                                                                  TState targetState,
                                                                  out TTransitionActionStatus transitionActionStatus,
                                                                  ExpandoObject  dto = default(ExpandoObject))
        {
            transitionActionStatus = default(TTransitionActionStatus); //ensures transitionActionStatus is reset before the transition
            if (!(statefulObject is ValueType) && statefulObject == null)
            {
                throw new ArgumentNullException("statefulObject");
            }

            if (targetState == null)
            {
                throw new ArgumentNullException("targetState");
            }

            dto = dto ?? new ExpandoObject();
            transitionActionStatus = StateMachine.TriggerTransition(targetState, statefulObject, dto);

            return statefulObject;
        }
    }
}