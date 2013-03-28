using System;
using NState.Test.Fast.BugTrackerExample.TransitionActions;

namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        [Serializable]
        public class Resolve : StateTransition<BugState, BugTransitionStatus>
        {
            public Resolve(BugTransitionAction.Resolve transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] StartStates
            {
                get { return new[] { new BugState.Assigned(), }; }
            }

            public override BugState[] EndStates
            {
                get { return new[] { new BugState.Resolved(), }; }
            }
        }
    }
}