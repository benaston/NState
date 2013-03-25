namespace NState
{
    public class NullStateMachine<TState, TTransitionStatus> : StateMachine<TState, TTransitionStatus> where TState : State
    {
        public override TTransitionStatus TriggerTransition(TState targetState, dynamic statefulObject, dynamic dto = null)
        {
            //do nothing, null object
            return default(TTransitionStatus);
        }
    }
}