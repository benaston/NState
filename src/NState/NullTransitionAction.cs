namespace NState
{
    public class NullTransitionAction<TState, TTransitionActionStatus> : TransitionAction<TState, TTransitionActionStatus> where TState : State
    {
        public override TTransitionActionStatus Run(TState targetState, 
                                                    IStateMachine<TState, TTransitionActionStatus> stateMachine, 
                                                    dynamic statefulObject, 
                                                    dynamic dto = default (dynamic))
        {
            //do nothing
            return default(TTransitionActionStatus);
        }
    }
}