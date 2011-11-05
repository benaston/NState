namespace NState.Test.Fast.BugTrackerExample
{
    public class Bug : Stateful<Bug, BugState>
    {
        public Bug(string title, IStateMachine<Bug, BugState> stateMachine)
            : base(stateMachine)
        {
            Title = title;
        }

        public string Title { get; set; }

        public string AssigneeEmail { get; set; }

        public string ClosedByName { get; set; }

        public void Assign(string assigneeEmail)
        {
            TriggerTransition(this, new BugState.Assigned(), new {AssigneeEmail = assigneeEmail});
        }

        public void Defer()
        {
            TriggerTransition(this, new BugState.Deferred());
        }

        public void Resolve()
        {
            TriggerTransition(this, new BugState.Resolved());
        }

        public void Close(string closedByName)
        {
            TriggerTransition(this, new BugState.Closed(), new {ClosedByName = closedByName});
        }
    }
}