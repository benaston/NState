namespace NState
{
    using System;

    [Serializable]
    public abstract class Stateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> :
        IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject :
            IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        protected Stateful(
            IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> StateMachine { get; set; }

        public TState CurrentState
        {
            get { return StateMachine.CurrentState; }
        }

        public TStatefulObject PerformTransition(TState targetState)
        {
            return
                StateMachine.PerformTransition(
                    (TStatefulObject)
                    ((IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>) this),
                    targetState);
        }
    }
}