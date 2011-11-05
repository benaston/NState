namespace NState.Test.Fast.BugTrackerExample
{
    public abstract class MyOtherBugTrackerState : MyAppState
    {
        public class Extinguished : BugTrackerState {}

        public class Smoking : BugTrackerState {}
    }
}