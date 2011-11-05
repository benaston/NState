namespace NState.Test.Fast.BugTrackerExample
{
    using System;

    public class BugHelper
    {
        public static void Assign(MyAppState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = args.AssigneeEmail;
        }

        public static void Defer(MyAppState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = String.Empty;
        }

        public static void Resolve(MyAppState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = String.Empty;
        }

        public static void Close(MyAppState state, dynamic args)
        {
            args.StatefulObject.ClosedByName = args.ClosedByName;
        }
    }
}