using System.Dynamic;

namespace NState
{
    /// <summary>
    /// Responsible for defining the interface for types 
    /// that may be used inside with the StateMachine type 
    /// (akin to workflow.)
    /// </summary>
    public interface IStateful<TState, TTransitionStatus> where TState : State
    {
        IStateMachine<TState, TTransitionStatus> StateMachine { get; set; }

        TState CurrentState { get; }

        TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject,
                                                           TState targetState,
                                                           out TTransitionStatus transitionStatus,
                                                           ExpandoObject dto);
    }
}