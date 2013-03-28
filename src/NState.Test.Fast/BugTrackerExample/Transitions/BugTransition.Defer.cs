using System;
using NState.Test.Fast.BugTrackerExample.TransitionActions;

namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        [Serializable]
        public class Defer : StateTransition<BugState, BugTransitionStatus>
        {
            public Defer(BugTransitionAction.Defer transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] StartStates
            {
                get { return new BugState[] { new BugState.Open(), new BugState.Assigned() }; }
            }

            public override BugState[] EndStates
            {
                get { return new[] { new BugState.Deferred(), }; }
            }
        }
    }
}