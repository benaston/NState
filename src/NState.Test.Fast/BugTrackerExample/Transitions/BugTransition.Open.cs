using System;

namespace NState.Test.Fast.BugTrackerExample.Transitions
{
    public partial class BugTransition
    {
        [Serializable]
        public class Open : StateTransition<BugState, BugTransitionStatus>
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