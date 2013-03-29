using System;

namespace NState.Test.Fast.BugTrackerExample.TransitionActions
{
    public partial class BugTransitionAction
    {
        public class Defer : TransitionAction<BugState, TransitionActionStatus>
        {
            public override TransitionActionStatus Run(BugState targetState,
                                                       IStateMachine<BugState, TransitionActionStatus> stateMachine,
                                                       dynamic statefulObject, 
                                                       dynamic dto = null)
            {
                statefulObject.Bug.AssigneeEmail = String.Empty;

                return TransitionActionStatus.Success;
            }
        }
    }
}