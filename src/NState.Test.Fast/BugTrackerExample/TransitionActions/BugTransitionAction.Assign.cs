using System;

namespace NState.Test.Fast.BugTrackerExample.TransitionActions
{
    public partial class BugTransitionAction
    {
        public class Assign : TransitionAction<BugState, BugTransitionStatus>
        {
            public override BugTransitionStatus Run(BugState targetState,
                                                    IStateMachine<BugState, BugTransitionStatus> stateMachine,
                                                    dynamic statefulObject, dynamic dto = null)
            {
                if (dto == null)
                {
                    throw new ArgumentNullException("dto");
                }

                if (dto.AssigneeEmail == null)
                {
                    throw new Exception("AssigneeEmail not supplied.");
                }

                statefulObject.Bug.AssigneeEmail = dto.AssigneeEmail;

                return BugTransitionStatus.Success;
            }
        }
    }
}