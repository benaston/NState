namespace NState
{
    using System;

    [Serializable]
    public abstract class Stateful<TStatefulObject, TState> :
        IStateful<TStatefulObject, TState>
        where TStatefulObject : IStateful<TStatefulObject, TState>
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

        public TStatefulObject PerformTransition(TState targetState, dynamic dto = default(dynamic))
        {
            return
                StateMachine.PerformTransition(
                    (TStatefulObject)
                    ((IStateful<TStatefulObject, TState>) this),
                    targetState, dto);
        }
    }
}