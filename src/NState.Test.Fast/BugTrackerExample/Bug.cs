namespace NState.Test.Fast.BugTrackerExample
{
    public class Bug
    {
        public Bug(string title) { Title = title; }

        public string Title { get; set; }

        public string AssigneeEmail { get; set; }

        public string ClosedByName { get; set; }
    }
}