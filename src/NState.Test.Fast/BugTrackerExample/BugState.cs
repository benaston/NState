namespace NState.Test.Fast.BugTrackerExample
{
    using System;

    [Serializable]
    public abstract class BugState : State
    {
        public class Assigned : BugState { }

        public class Closed : BugState { }

        public class Deferred : BugState { }

        public class Open : BugState { }

        public class Resolved : BugState { }
    }
}