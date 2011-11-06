namespace NState
{
    using System;
    using System.Dynamic;
    using NSure;
    using ArgumentException = NHelpfulException.FrameworkExceptions.ArgumentException;

    /// <summary>
    ///   Inherit from this if you want to make your type stateful.
    /// </summary>
    public abstract class Stateful<TStatefulObject, TState> :
        IStateful<TStatefulObject, TState>
        where TStatefulObject : Stateful<TStatefulObject, TState>
        where TState : State
    {
        protected Stateful(
            IStateMachine<TState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public TState ParentState
        {
            get { return StateMachine.Parent.CurrentState; }
        }

        public IStateMachine<TState> StateMachine { get; set; }

        public TState CurrentState
        {
            get { return StateMachine.CurrentState; }
        }

        public TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject, TState targetState,
                                                                  ExpandoObject dto = default(ExpandoObject))
        {
            Ensure.That<ArgumentException>(statefulObject is ValueType ? true : statefulObject != null, "statefulObject not supplied.")
                .And<ArgumentException>(targetState != null, "targetState not supplied.");

            dto = dto ?? new ExpandoObject();
            ((dynamic) dto).StatefulObject = statefulObject;
            StateMachine.TriggerTransition(targetState, dto);

            return statefulObject;
        }
    }
}