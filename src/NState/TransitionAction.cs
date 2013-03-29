namespace NState
{
    /// <summary>
    /// Examples of actions 
    /// being performed during a transition include the sending 
    /// of an email or SMS, the persisting of information to 
    /// the database, or an onscreen notification or the billing 
    /// of funds.
    /// </summary>
    public abstract class TransitionAction<TState, TTransitionActionStatus> where TState : State
    {
        /// <param name="targetState">The state being transitioned to.</param>
        /// <param name="stateMachine">The statemachine governing the transition.</param>
        /// <param name="statefulObject">The stateful object undergoing the transition.</param>
        /// <param name="dto">A DTO containing any additional data required by the transition.</param>
        /// <returns></returns>
        public abstract TTransitionActionStatus Run(TState targetState, 
                                                    IStateMachine<TState, TTransitionActionStatus> stateMachine, 
                                                    dynamic statefulObject, 
                                                    dynamic dto = default(dynamic));
    }
}