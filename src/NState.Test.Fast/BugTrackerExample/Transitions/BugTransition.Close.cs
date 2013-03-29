using NState.Test.Fast.BugTrackerExample.TransitionActions;

namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        public class Close : StateTransition<BugState, TransitionActionStatus>
        {
            public Close(BugTransitionAction.Close transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] StartStates
            {
                get { return new BugState[] { new BugState.Resolved() }; }
            }

            public override BugState[] EndStates
            {
                get { return new[] { new BugState.Closed(), }; }
            }
        }
    }
}