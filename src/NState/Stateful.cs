using System;
using System.Dynamic;
using NState.Exceptions;

namespace NState
{
    /// <summary>
    /// Inherit from this if you want to make your type stateful.
    /// </summary>
    public abstract class Stateful<TState, TTransitionStatus> : IStateful<TState, TTransitionStatus> where TState : State
    {
        protected Stateful(IStateMachine<TState, TTransitionStatus> stateMachine)
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

        public IStateMachine<TState, TTransitionStatus> StateMachine { get; set; }

        public TState CurrentState
        {
            get { return StateMachine.CurrentState; }
        }

        public TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject,
                                                                  TState targetState,
                                                                  out TTransitionStatus transitionStatus,
                                                                  ExpandoObject  dto = default(ExpandoObject))
        {
            if (!(statefulObject is ValueType) && statefulObject == null)
            {
                throw new ArgumentNullException("statefulObject");
            }

            if (targetState == null)
            {
                throw new ArgumentNullException("targetState");
            }

            dto = dto ?? new ExpandoObject();
            transitionStatus = StateMachine.TriggerTransition(targetState, statefulObject, dto);

            return statefulObject;
        }
    }
}