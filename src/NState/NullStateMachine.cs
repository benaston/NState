namespace NState
{
    public class NullStateMachine<TState, TTransitionActionStatus> : StateMachine<TState, TTransitionActionStatus> where TState : State
    {
        public override TTransitionActionStatus TriggerTransition(TState targetState, 
                                                                  dynamic statefulObject, 
                                                                  dynamic dto = null)
        {
            //do nothing, null object
            return default(TTransitionActionStatus);
        }
    }
}