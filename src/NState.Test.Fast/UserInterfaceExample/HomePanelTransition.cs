namespace NState.Test.Fast.UserInterfaceExample
{
    using System;

    public class HomePanelTransition
    {
        [Serializable]
        public class Hide : StateTransition<LucidState>
        {
            public Hide(Action<LucidState, object> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new HomePanelState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new HomePanelState.Hidden(),}; }
            }
        }

        [Serializable]
        public class Show : StateTransition<LucidState>
        {
            public Show(Action<LucidState, object> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new HomePanelState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new HomePanelState.Visible(),}; }
            }
        }
    }
}