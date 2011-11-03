namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class SavedSearch : Stateful<SavedSearch, SavedSearchState, StateMachineType>
    {
        public SavedSearch(
            IStateMachine<SavedSearch, SavedSearchState, StateMachineType> stateMachine,
            LucidUI uiContext) : base(stateMachine)
        {
            UIContext = uiContext;
        }

        public string Id { get; set; }

        public LucidUI UIContext { get; set; }
    }
}