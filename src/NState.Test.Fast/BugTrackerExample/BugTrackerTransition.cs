namespace NState.Test.Fast.BugTrackerExample
{
    using System;

    public class BugTrackerTransition
    {
        [Serializable]
        public class Extinguish : StateTransition<MyAppState>
        {
            public Extinguish(Action<MyAppState, object> transitionFunction = null) : base(transitionFunction) {}

            public override MyAppState[] StartStates
            {
                get { return new[] {new BugTrackerState.Extinguished(),}; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] {new BugTrackerState.Extinguished(),}; }
            }
        }

        [Serializable]
        public class SetAlight : StateTransition<MyAppState>
        {
            public SetAlight(Action<MyAppState, object> transitionFunction = null) : base(transitionFunction) {}

            public override MyAppState[] StartStates
            {
                get { return new[] {new BugTrackerState.Extinguished(),}; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] {new BugTrackerState.Smoking(),}; }
            }
        }
    }
}