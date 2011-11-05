namespace NState.Test.Fast.BugTrackerExample
{
    public abstract class BugTrackerState : MyAppState
    {
        public class Extinguished : BugTrackerState {}

        public class Smoking : BugTrackerState {}
    }
}