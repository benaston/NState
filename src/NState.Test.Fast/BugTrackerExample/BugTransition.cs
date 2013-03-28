namespace NState.Test.Fast.BugTrackerExample
{
    using System;

    [Serializable]
    public class BugTransition
    {
        [Serializable]
        public class Assign : StateTransition<BugState, BugTransitionStatus>
        {
            public Assign(BugTransitionAction.Assign transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] InitialStates
            {
                get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Assigned(),}; }
            }
        }

        [Serializable]
        public class Close : StateTransition<BugState, BugTransitionStatus>
        {
            public Close(BugTransitionAction.Close transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] InitialStates
            {
                get { return new BugState[] {new BugState.Resolved()}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Closed(),}; }
            }
        }

        [Serializable]
        public class Defer : StateTransition<BugState, BugTransitionStatus>
        {
            public Defer(BugTransitionAction.Defer transitionAction)
                : base(transitionAction:transitionAction) { }

            public override BugState[] InitialStates
            {
                get { return new BugState[] {new BugState.Open(), new BugState.Assigned()}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Deferred(),}; }
            }
        }

        [Serializable]
        public class Open : StateTransition<BugState, BugTransitionStatus>
        {
            public override BugState[] InitialStates
            {
                get { return new[] {new BugState.Closed(),}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Open(),}; }
            }
        }

        [Serializable]
        public class Resolve : StateTransition<BugState, BugTransitionStatus>
        {
            public Resolve(BugTransitionAction.Resolve transitionAction)
                : base(transitionAction: transitionAction) { }

            public override BugState[] InitialStates
            {
                get { return new[] {new BugState.Assigned(),}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Resolved(),}; }
            }
        }
    }
}