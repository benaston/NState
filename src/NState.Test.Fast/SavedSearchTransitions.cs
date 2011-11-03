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

            public override SavedSearchState[] StartState
            {
                get { return new[] { new SavedSearchState.Expanded(), }; }
            }

            public override SavedSearchState[] EndState
            {
                get { return new[] { new SavedSearchState.Collapsed(), }; }
            }
        }

        [Serializable]
        public class Expand : StateTransition<SavedSearch, SavedSearchState>
        {
            public Expand(Func<SavedSearch, SavedSearchState, dynamic, SavedSearch> transitionFunction)
                : base(transitionFunction) {}

            public override SavedSearchState[] StartState
            {
                get { return new[] { new SavedSearchState.Collapsed(), }; }
            }

            public override SavedSearchState[] EndState
            {
                get { return new[] { new SavedSearchState.Expanded(), }; }
            }
        }
    }
}