namespace NState.Test.Fast.BugTrackerExample
{
    using System.Dynamic;

    public class Bug : Stateful<BugState, BugTransitionStatus>
    {
        public Bug(string title, IStateMachine<BugState, BugTransitionStatus> stateMachine)
            : base(stateMachine) { Title = title; }

        public string Title { get; set; }

        public string AssigneeEmail { get; set; }

        public string ClosedByName { get; set; }

        public Bug Open()
        {
            BugTransitionStatus transitionStatus;

            return TriggerTransition(this, new BugState.Open(), out transitionStatus);
        }

        public Bug Assign(string assigneeEmail)
        {
            dynamic args = new ExpandoObject();
            args.AssigneeEmail = assigneeEmail;
            BugTransitionStatus transitionStatus;

            return TriggerTransition(this, new BugState.Assigned(), out transitionStatus, args);
        }

        public Bug Defer()
        {
            BugTransitionStatus transitionStatus;

            return TriggerTransition(this, new BugState.Deferred(), out transitionStatus);
        }

        public Bug Resolve()
        {
            BugTransitionStatus transitionStatus;

            return TriggerTransition(this, new BugState.Resolved(), out transitionStatus);
        }

        public Bug Close(string closedByName)
        {
            dynamic args = new ExpandoObject();
            args.ClosedByName = closedByName;
            BugTransitionStatus transitionStatus;

            return TriggerTransition(this, new BugState.Closed(), out transitionStatus, args);
        }
    }
}