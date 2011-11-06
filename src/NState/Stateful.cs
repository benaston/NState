namespace NState
{
    using System;
    using System.Dynamic;

    /// <summary>
    ///   Inherit from this if you want to make your object stateful.
    /// </summary>
    /// <typeparam name = "TStatefulObject">The type of your stateful object.</typeparam>
    /// <typeparam name = "TState">The type you are using to define the state for your type.</typeparam>
    [Serializable]
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
            dto = dto ?? new ExpandoObject();
            ((dynamic) dto).StatefulObject = statefulObject;
            StateMachine.TriggerTransition(targetState, dto);

            return statefulObject;
        }
    }
}