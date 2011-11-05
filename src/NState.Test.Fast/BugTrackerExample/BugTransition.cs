namespace NState.Test.Fast.BugTrackerExample
{
    using System;

    [Serializable]
    public class BugTransition
    {
        [Serializable]
        public class Assign : StateTransition<BugState>
        {
            public Assign(Action<BugState, object> transitionFunction = null) : base(transitionFunction) {}

            public override BugState[] StartStates
            {
                get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Assigned(),}; }
            }
        }

        [Serializable]
        public class Close : StateTransition<BugState>
        {
            public Close(Action<BugState, object> transitionFunction = null) : base(transitionFunction) {}

            public override BugState[] StartStates
            {
                get { return new BugState[] {new BugState.Resolved()}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Closed(),}; }
            }
        }

        [Serializable]
        public class Defer : StateTransition<BugState>
        {
            public Defer(Action<BugState, object> transitionFunction = null) : base(transitionFunction) {}

            public override BugState[] StartStates
            {
                get { return new BugState[] {new BugState.Open(), new BugState.Assigned()}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Deferred(),}; }
            }
        }

        [Serializable]
        public class Open : StateTransition<BugState>
        {
            public Open(Action<BugState, object> transitionFunction = null) : base(transitionFunction) {}

            public override BugState[] StartStates
            {
                get { return new[] {new BugState.Closed(),}; }
            }

            public override BugState[] EndStates
            {
                get { return new[] {new BugState.Open(),}; }
            }
        }

        [Serializable]
        public class Resolve : StateTransition<BugState>
        {
            public Resolve(Action<BugState, object> transitionFunction = null) : base(transitionFunction) {}

            public override BugState[] StartStates
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