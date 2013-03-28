using NState.Test.Fast.BugTrackerExample.TransitionActions;

namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        public class Assign : StateTransition<BugState, TransitionStatus>
        {
            public Assign(BugTransitionAction.Assign transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] StartStates
            {
                get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Assigned(),}; }
            }
        }
    }
}