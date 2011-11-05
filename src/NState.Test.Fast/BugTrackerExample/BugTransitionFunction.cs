namespace NState.Test.Fast.BugTrackerExample
{
    using System;

    public class BugTransitionFunction
    {
        public static void Assign(BugState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = args.AssigneeEmail;
        }

        public static void Defer(BugState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = String.Empty;
        }

        public static void Resolve(BugState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = String.Empty;
        }

        public static void Close(BugState state, dynamic args)
        {
            args.StatefulObject.ClosedByName = args.ClosedByName;
        }
    }
}