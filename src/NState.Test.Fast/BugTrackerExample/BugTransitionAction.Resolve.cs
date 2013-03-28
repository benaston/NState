using System;

namespace NState.Test.Fast.BugTrackerExample
{
    public partial class BugTransitionAction
    {
        public class Resolve : TransitionAction<BugState, BugTransitionStatus>
        {
            public override BugTransitionStatus Run(BugState targetState,
                                                    IStateMachine<BugState, BugTransitionStatus> stateMachine,
                                                    dynamic statefulObject, dynamic dto = null)
            {
                statefulObject.AssigneeEmail = String.Empty;

                return BugTransitionStatus.Success;
            }
        }
    }
}