namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        public class Open : StateTransition<BugState, TransitionActionStatus>
        {
            public override BugState[] StartStates
            {
                get { return new[] { new BugState.Closed(), }; }
            }

            public override BugState[] EndStates
            {
                get { return new[] { new BugState.Open(), }; }
            }
        }
    }
}