using System.Dynamic;

namespace NState.Test.Fast.BugTrackerExample
{
    /// <summary>
    /// This wrapper type exposes the stateful functionality for Bugs.
    /// This functionality could be placed on the Bug type itself at the 
    /// expense of some mixing of concerns.
    /// </summary>
    public class BugStateful : Stateful<BugState, TransitionStatus>
    {
        public BugStateful(Bug bug, IStateMachine<BugState, TransitionStatus> stateMachine = null)
            : base(stateMachine ?? new NullStateMachine<BugState, TransitionStatus>()) { Bug = bug; }

        public Bug Bug { get; set; }

        public BugStateful Open(out TransitionStatus transitionStatus)
        {
            return TriggerTransition(this, new BugState.Open(), out transitionStatus);
        }

        public BugStateful Assign(string assigneeEmail, out TransitionStatus transitionStatus)
        {
            dynamic dto = new ExpandoObject();
            dto.AssigneeEmail = assigneeEmail;

            return TriggerTransition(this, new BugState.Assigned(), out transitionStatus, dto);
        }

        public BugStateful Defer(out TransitionStatus transitionStatus)
        {
            return TriggerTransition(this, new BugState.Deferred(), out transitionStatus);
        }

        public BugStateful Resolve(out TransitionStatus transitionStatus)
        {
            return TriggerTransition(this, new BugState.Resolved(), out transitionStatus);
        }

        public BugStateful Close(string closedByName, out TransitionStatus transitionStatus)
        {
            dynamic dto = new ExpandoObject();
            dto.ClosedByName = closedByName;

            return TriggerTransition(this, new BugState.Closed(), out transitionStatus, dto);
        }
    }
}