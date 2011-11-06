namespace NState.Test.Fast.UserInterfaceExample
{
    using System;

    public class SearchAreaTransition
    {
        [Serializable]
        public class Hide : StateTransition<LucidState>
        {
            public Hide(Action<LucidState, IStateMachine<LucidState>, dynamic> transitionFunction = null)
                : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new SearchAreaState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new SearchAreaState.Hidden(),}; }
            }
        }

        [Serializable]
        public class Show : StateTransition<LucidState>
        {
            public Show(Action<LucidState, IStateMachine<LucidState>, dynamic> transitionFunction = null)
                : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new SearchAreaState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new SearchAreaState.Visible(),}; }
            }
        }
    }
}