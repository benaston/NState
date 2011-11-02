namespace NState.Test.Fast
{
    public class SavedSearch : IStateful<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>
    {
        public SavedSearch(LucidUI uiContext)
        {
            UiContext = uiContext;
        }

        public LucidUI UiContext { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// See comment on iface.
        /// </summary>
        public IStateMachine<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType> GetStateMachine(
            IStateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType> stateMachine)
        {
            return
                ((IStateMachine<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>)
                 (stateMachine.ChildStateMachines[StateMachineType.SavedSearch]));
        }

        public SavedSearchState CurrentState { get; set; }
    }
}