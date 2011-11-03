namespace NState
{
    using System;

    [Serializable]
    public abstract class Stateful<TStatefulObject, TState, TStateMachineTypeEnum> :
        IStateful<TStatefulObject, TState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TStateMachineTypeEnum>
        where TState : State
        where TStateMachineTypeEnum : struct
    {
        protected Stateful(
            IStateMachine<TStatefulObject, TState, TStateMachineTypeEnum> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public IStateMachine<TStatefulObject, TState, TStateMachineTypeEnum> StateMachine { get; set; }

        public TState CurrentState
        {
            get { return StateMachine.CurrentState; }
        }

        public TStatefulObject PerformTransition(TState targetState)
        {
            return
                StateMachine.PerformTransition(
                    (TStatefulObject)
                    ((IStateful<TStatefulObject, TState, TStateMachineTypeEnum>) this),
                    targetState);
        }
    }
}