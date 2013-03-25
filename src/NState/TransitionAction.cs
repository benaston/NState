namespace NState
{
    /// <summary>
    /// Examples of actions 
    /// being performed during a transition include the sending 
    /// of an email or SMS, the persisting of information to 
    /// the database, or an onscreen notification or the billing 
    /// of funds.
    /// </summary>
    public abstract class TransitionAction<TState, TTransitionStatus> where TState : State
    {
        public abstract TTransitionStatus Run(TState targetState, IStateMachine<TState, TTransitionStatus> stateMachine, dynamic statefulObject, dynamic dto = default(dynamic));
    }
}