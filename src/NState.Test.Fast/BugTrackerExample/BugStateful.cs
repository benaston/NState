using System.Dynamic;

namespace NState.Test.Fast.BugTrackerExample
{
    /// <summary>
    /// This wrapper type exposes the stateful functionality for Bugs.
    /// This functionality could be placed on the Bug type itself at the 
    /// expense of some mixing of concerns.
    /// </summary>
    public class BugStateful : Stateful<BugState, BugTransitionStatus>
    {
        public BugStateful(Bug bug, IStateMachine<BugState, BugTransitionStatus> stateMachine = null)
            : base(stateMachine ?? new NullStateMachine<BugState, BugTransitionStatus>()) { Bug = bug; }

        public Bug Bug { get; set; }

        public BugStateful Open(out BugTransitionStatus transitionStatus)
        {
            return TriggerTransition(this, new BugState.Open(), out transitionStatus);
        }

        public BugStateful Assign(string assigneeEmail, out BugTransitionStatus transitionStatus)
        {
            dynamic dto = new ExpandoObject();
            dto.AssigneeEmail = assigneeEmail;

            return TriggerTransition(this, new BugState.Assigned(), out transitionStatus, dto);
        }

        public BugStateful Defer(out BugTransitionStatus transitionStatus)
        {
            return TriggerTransition(this, new BugState.Deferred(), out transitionStatus);
        }

        public BugStateful Resolve(out BugTransitionStatus transitionStatus)
        {
            return TriggerTransition(this, new BugState.Resolved(), out transitionStatus);
        }

        public BugStateful Close(string closedByName, out BugTransitionStatus transitionStatus)
        {
            dynamic args = new ExpandoObject();
            args.ClosedByName = closedByName;

            return TriggerTransition(this, new BugState.Closed(), out transitionStatus, args);
        }
    }
}