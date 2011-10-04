namespace NState
{
    using System;

    /// <summary>
    ///   Defines the interface for a state transition for a type that supports
    ///   "workflow". Examples of actions being performed during a transition
    ///   include the sending of an email or SMS, the persisting of information 
    ///   to the database, or an onscreen notification or the billing of funds.
    ///   Note that a computer program might invoke the state transition.
    /// </summary>
    public interface IStateTransition<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TStatefulObject : IStateful<TStatefulObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
                                                 where TState : State
        where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        where TBaseState : State
        where TStateMachineTypeEnum : struct
    {
        TState StartState { get; }
        TState EndState { get; }
        Func<TStatefulObject, TState, TStatefulObject> Transition { get; }
    }
}