namespace NState
{
    using System;

    /// <summary>
    ///   Responsible for defining the interface for types 
    ///   that may be used inside with the StateMachine type 
    ///   (akin to workflow.)
    /// </summary>
    public interface IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> 
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        ///// <summary>
        ///// Given a stateful domain object retrieves the state machine relevant to it
        ///// from the parent state composite object.
        ///// </summary>
        //IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> 
        //    GetStateMachine(IStateMachine<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> stateMachine);

        IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> StateMachine { get; set; }

        TStatefulObject PerformTransition(TStatefulObject statefulDomainObject, TState targetState);

        TState CurrentState { get; }
    }

    [Serializable]
    public abstract class Stateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> 
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        protected Stateful(IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> StateMachine { get; set; }

        public TState CurrentState { get { return StateMachine.CurrentState; } }

        //convenience method / refactor!
        public TStatefulObject PerformTransition(TStatefulObject statefulDomainObject, TState targetState)
        {
            return StateMachine.PerformTransition(statefulDomainObject, targetState);
            //return StateMachine.PerformTransition((TStatefulObject)this, targetState);
        }
    }
}