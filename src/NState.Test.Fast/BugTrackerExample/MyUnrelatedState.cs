namespace NState.Test.Fast.BugTrackerExample
{
    public abstract class MyUnrelatedState : State
    {
        public class Extinguished : BugTrackerState {}

        public class Smoking : BugTrackerState {}
    }
}