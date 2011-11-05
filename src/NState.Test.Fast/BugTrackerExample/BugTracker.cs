namespace NState.Test.Fast.BugTrackerExample
{
    public class BugTracker : Stateful<BugTracker, MyAppState>
    {
        public BugTracker(IStateMachine<BugTracker, MyAppState> stateMachine)
            : base(stateMachine) {}
    }
}