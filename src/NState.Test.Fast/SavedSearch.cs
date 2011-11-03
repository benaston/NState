namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class SavedSearch : Stateful<SavedSearch, SavedSearchState>
    {
        public SavedSearch(
            IStateMachine<SavedSearch, SavedSearchState> stateMachine,
            LucidUI uiContext) : base(stateMachine)
        {
            UIContext = uiContext;
        }

        public string Id { get; set; }

        public LucidUI UIContext { get; set; }
    }
}