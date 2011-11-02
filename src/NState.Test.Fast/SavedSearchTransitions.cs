namespace NState.Test.Fast
{
    using System;

    public class SavedSearchTransitions
    {
        public class Expand : StateTransition<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>
        {
            public Expand(Func<SavedSearch, SavedSearchState, SavedSearch> transitionFunction) : base(transitionFunction) { }

            public override SavedSearchState StartState
            {
                get { return new SavedSearchState.Collapsed(); }
            }

            public override SavedSearchState EndState
            {
                get { return new SavedSearchState.Expanded(); }

            }
        }

        public class Collapse : StateTransition<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>
        {
            public Collapse(Func<SavedSearch, SavedSearchState, SavedSearch> transitionFunction) : base(transitionFunction) { }

            public override SavedSearchState StartState
            {
                get { return new SavedSearchState.Expanded(); }
            }

            public override SavedSearchState EndState
            {
                get { return new SavedSearchState.Collapsed(); }

            }
        }
    }
}