namespace NState
{
    /// <summary>
    /// Base for types encapsulating logic 
    /// to be run when a transition occurs.
    /// </summary>
    public abstract class TransitionAction<TState, TTransitionActionStatus> where TState : State
    {
        /// <param name="targetState">The state being transitioned to.</param>
        /// <param name="stateMachine">The statemachine governing the transition.</param>
        /// <param name="statefulObject">The stateful object undergoing the transition.</param>
        /// <param name="dto">A DTO containing any additional data required by the transition.</param>
        /// <returns>An indication of the status of the transition (success, failure, warning etc.).</returns>
        public abstract TTransitionActionStatus Run(TState targetState, 
                                                    IStateMachine<TState, TTransitionActionStatus> stateMachine, 
                                                    dynamic statefulObject, 
                                                    dynamic dto = default(dynamic));
    }
}