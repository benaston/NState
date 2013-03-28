using System;

namespace NState.Test.Fast.BugTrackerExample.TransitionActions
{
    public partial class BugTransitionAction
    {
        public class Close : TransitionAction<BugState, TransitionStatus>
        {
            public override TransitionStatus Run(BugState targetState,
                                                    IStateMachine<BugState, TransitionStatus> stateMachine,
                                                    dynamic statefulObject, dynamic dto = null)
            {
                if (dto == null)
                {
                    throw new ArgumentNullException("dto");
                }

                if (dto.ClosedByName == null)
                {
                    throw new Exception("ClosedByName not supplied.");
                }

                statefulObject.StatefulObject.Bug.ClosedByName = dto.ClosedByName;

                return TransitionStatus.Success;
            }
        }
    }
}