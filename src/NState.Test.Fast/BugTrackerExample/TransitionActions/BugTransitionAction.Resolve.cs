using System;

namespace NState.Test.Fast.BugTrackerExample.TransitionActions
{
    public partial class BugTransitionAction
    {
        public class Resolve : TransitionAction<BugState, TransitionStatus>
        {
            public override TransitionStatus Run(BugState targetState,
                                                    IStateMachine<BugState, TransitionStatus> stateMachine,
                                                    dynamic statefulObject, dynamic dto = null)
            {
                statefulObject.Bug.AssigneeEmail = String.Empty;

                return TransitionStatus.Success;
            }
        }
    }
}