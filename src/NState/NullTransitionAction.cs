namespace NState
{
    public class NullTransitionAction<TState, TTransitionStatus> : TransitionAction<TState, TTransitionStatus> where TState : State
    {
        public override TTransitionStatus Run(TState targetState, IStateMachine<TState, TTransitionStatus> stateMachine, dynamic statefulObject, dynamic dto = default (dynamic))
        {
            //do nothing
            return default(TTransitionStatus);
        }
    }
}