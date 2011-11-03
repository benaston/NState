namespace NState
{
    using System;
    using System.Collections.Generic;

    public interface IStateMachine {}

    /// <summary>
    ///   Responsible for defining the interface for types that
    ///   control the transitions between state machine states.
    /// </summary>
    public interface IStateMachine<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> :
        IStateMachine
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TState : State
        where TBaseDomainObject :
            IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        List<IStateMachine> ChildStateMachines { get; set; }

        List<IStateMachine> ParentStateMachines { get; set; }

        TState StartState { get; set; }

        TState CurrentState { get; set; }

        Dictionary
            <DateTime, IStateTransition<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>>
            History { get; set; }

        TStatefulObject PerformTransition(TStatefulObject opportunity, TState targetState);
    }
}