using System.Dynamic;

namespace NState
{
    /// <summary>
    /// Implemented by types that have a 
    /// corresponding StateMachine.
    /// </summary>
    internal interface IStateful<TState, TTransitionActionStatus> where TState : State
    {
        /// <summary>
        /// The state machine governing transitions for this stateful object.
        /// </summary>
        IStateMachine<TState, TTransitionActionStatus> StateMachine { get; set; }

        /// <summary>
        /// The current state of this stateful object.
        /// </summary>
        TState CurrentState { get; }

        /// <summary>
        /// Perform the specified transition against this stateful object.
        /// </summary>
        /// <typeparam name="TStatefulObject">The type of the stateful object undergoing the transition.</typeparam>
        /// <param name="statefulObject">The stateful object undergoing the transition.</param>
        /// <param name="targetState">The state being transitioned to.</param>
        /// <param name="transitionActionStatus">A reference to the status of any action associated with the transition.</param>
        /// <param name="dto">A DTO containing any additional data required to perform the transition.</param>
        /// <returns>The object having undergone the transition attempt (success/failure indicated by transition action status).</returns>
        TStatefulObject TriggerTransition<TStatefulObject>(TStatefulObject statefulObject,
                                                           TState targetState,
                                                           out TTransitionActionStatus transitionActionStatus,
                                                           ExpandoObject dto);
    }
}