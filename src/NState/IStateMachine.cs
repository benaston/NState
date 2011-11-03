namespace NState
{
    using System;
    using System.Collections.Generic;

    public interface IStateMachine {}

    /// <summary>
    ///   Responsible for defining the interface for types that
    ///   control the transitions between state machine states.
    /// </summary>
    public interface IStateMachine<TStatefulObject, TState, TStateMachineTypeEnum> :
        IStateMachine
        where TStatefulObject : IStateful<TStatefulObject, TState, TStateMachineTypeEnum>
        where TState : State
        where TStateMachineTypeEnum : struct
    {
        TState StartState { get; set; }

        TState CurrentState { get; set; }

        List<IStateMachine> ChildStateMachines { get; set; }

        List<IStateMachine> ParentStateMachines { get; set; }

        Dictionary
            <DateTime, IStateTransition<TStatefulObject, TState, TStateMachineTypeEnum>>
            History { get; set; }

        TStatefulObject PerformTransition(TStatefulObject opportunity, TState targetState);
    }
}