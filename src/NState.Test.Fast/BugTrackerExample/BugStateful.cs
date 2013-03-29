using System.Dynamic;

namespace NState.Test.Fast.BugTrackerExample
{
    /// <summary>
    /// This wrapper type exposes the stateful functionality for Bugs.
    /// This functionality could be placed on the Bug type itself at the 
    /// expense of some mixing of concerns.
    /// </summary>
    public class BugStateful : Stateful<BugState, TransitionActionStatus>
    {
        public BugStateful(Bug bug, IStateMachine<BugState, TransitionActionStatus> stateMachine = null)
            : base(stateMachine ?? new NullStateMachine<BugState, TransitionActionStatus>()) { Bug = bug; }

        public Bug Bug { get; set; }

        public BugStateful Open(out TransitionActionStatus transitionActionStatus)
        {
            return TriggerTransition(this, new BugState.Open(), out transitionActionStatus);
        }

        public BugStateful Assign(string assigneeEmail, out TransitionActionStatus transitionActionStatus)
        {
            dynamic dto = new ExpandoObject();
            dto.AssigneeEmail = assigneeEmail;

            return TriggerTransition(this, new BugState.Assigned(), out transitionActionStatus, dto);
        }

        public BugStateful Defer(out TransitionActionStatus transitionActionStatus)
        {
            return TriggerTransition(this, new BugState.Deferred(), out transitionActionStatus);
        }

        public BugStateful Resolve(out TransitionActionStatus transitionActionStatus)
        {
            return TriggerTransition(this, new BugState.Resolved(), out transitionActionStatus);
        }

        public BugStateful Close(string closedByName, out TransitionActionStatus transitionActionStatus)
        {
            dynamic dto = new ExpandoObject();
            dto.ClosedByName = closedByName;

            return TriggerTransition(this, new BugState.Closed(), out transitionActionStatus, dto);
        }
    }
}