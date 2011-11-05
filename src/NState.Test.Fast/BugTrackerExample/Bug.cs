namespace NState.Test.Fast.BugTrackerExample
{
    public class Bug : Stateful<Bug, BugState>
    {
        public Bug(string title, IStateMachine<BugState> stateMachine)
            : base(stateMachine)
        {
            Title = title;
        }

        public string Title { get; set; }

        public string AssigneeEmail { get; set; }

        public string ClosedByName { get; set; }

        public Bug Open()
        {
            return TriggerTransition(this, new BugState.Open());
        }

        public Bug Assign(string assigneeEmail)
        {
            return TriggerTransition(this, new BugState.Assigned(), new { StatefulObject = this, AssigneeEmail = assigneeEmail});
        }

        public Bug Defer()
        {
            return TriggerTransition(this, new BugState.Deferred(), new { StatefulObject = this });
        }

        public Bug Resolve()
        {
            return TriggerTransition(this, new BugState.Resolved());
        }

        public Bug Close(string closedByName)
        {
            return TriggerTransition(this, new BugState.Closed(), new { StatefulObject = this, ClosedByName = closedByName });
        }
    }
}