using System;
using NState.Test.Fast.BugTrackerExample.TransitionActions;

namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        [Serializable]
        public class Close : StateTransition<BugState, BugTransitionStatus>
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