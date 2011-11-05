namespace NState.Test.Fast.BugTrackerExample
{
    using System.Dynamic;

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
            dynamic args = new ExpandoObject();
            args.AssigneeEmail = assigneeEmail;

            return TriggerTransition(this, new BugState.Assigned(), args);
        }

        public Bug Defer()
        {
            return TriggerTransition(this, new BugState.Deferred());
        }

        public Bug Resolve()
        {
            return TriggerTransition(this, new BugState.Resolved());
        }

        public Bug Close(string closedByName)
        {
            dynamic args = new ExpandoObject();
            args.ClosedByName = closedByName;

            return TriggerTransition(this, new BugState.Closed(), args);
        }
    }
}