namespace NState
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public interface IStateMachine {}

    /// <summary>
    ///   Responsible for defining the interface for types that
    ///   control the transitions between state machine states.
    /// </summary>
    public interface IStateMachine<TDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> : IStateMachine, ISerializable
        where TDomainObject : IStateful<TDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        Dictionary<TStateMachineTypeEnum, IStateMachine> ChildStateMachines { get; set; }
        TState StartState { get; set; }
        TState CurrentState { get; set; }
        Dictionary<DateTime, IStateTransition<TDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>> History { get; set; }
        TDomainObject PerformTransition(TDomainObject opportunity, TState targetState);
    }
}