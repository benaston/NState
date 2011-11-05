namespace NState
{
    using System;

    /// <summary>
    /// Inherit from this if you want to make your object stateful.
    /// </summary>
    /// <typeparam name="TStatefulObject">The type of your stateful object.</typeparam>
    /// <typeparam name="TState">The type you are using to define the state for your type.</typeparam>
    [Serializable]
    public abstract class Stateful<TStatefulObject, TState> :
        IStateful<TStatefulObject, TState>
        where TStatefulObject : Stateful<TStatefulObject, TState>
        where TState : State
    {
        protected Stateful(
            IStateMachine<TStatefulObject, TState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public IStateMachine<TStatefulObject, TState> StateMachine { get; set; }

        public TState CurrentState
        {
            get { return StateMachine.CurrentState; }
        }

        public TState ParentState
        {
            get { return StateMachine.Parent.CurrentState; }
        }

        //supply stateful object automatically?
        public TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject, TState targetState, dynamic dto = default(dynamic))
        {
            StateMachine.TriggerTransition(targetState, dto);

            return statefulObject;
        }
    }
}