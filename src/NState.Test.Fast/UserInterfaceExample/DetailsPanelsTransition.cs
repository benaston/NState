namespace NState.Test.Fast.UserInterfaceExample
{
    using System;

    public class DetailsPanelsTransition
    {
        [Serializable]
        public class SelectSearchMode : StateTransition<LucidState>
        {
            public SelectSearchMode(Action<LucidState, IStateMachine<LucidState>, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new DetailsPanelsState.AccountMode(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new DetailsPanelsState.SearchMode(), }; }
            }
        }

        [Serializable]
        public class SelectAccountMode : StateTransition<LucidState>
        {
            public SelectAccountMode(Action<LucidState, IStateMachine<LucidState>, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new DetailsPanelsState.SearchMode(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new DetailsPanelsState.AccountMode(), }; }
            }
        }
    }
}