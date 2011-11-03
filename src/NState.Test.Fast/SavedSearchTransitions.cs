namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class SavedSearchTransitions
    {
        [Serializable]
        public class Collapse : StateTransition<SavedSearch, SavedSearchState>
        {
            public Collapse(Func<SavedSearch, SavedSearchState, dynamic, SavedSearch> transitionFunction)
                : base(transitionFunction) {}

            public override SavedSearchState[] StartStates
            {
                get { return new[] { new SavedSearchState.Expanded(), }; }
            }

            public override SavedSearchState[] EndStates
            {
                get { return new[] { new SavedSearchState.Collapsed(), }; }
            }
        }

        [Serializable]
        public class Expand : StateTransition<SavedSearch, SavedSearchState>
        {
            public Expand(Func<SavedSearch, SavedSearchState, dynamic, SavedSearch> transitionFunction)
                : base(transitionFunction) {}

            public override SavedSearchState[] StartStates
            {
                get { return new[] { new SavedSearchState.Collapsed(), }; }
            }

            public override SavedSearchState[] EndStates
            {
                get { return new[] { new SavedSearchState.Expanded(), }; }
            }
        }
    }
}