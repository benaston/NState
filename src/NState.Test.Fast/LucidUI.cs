namespace NState.Test.Fast
{
    using System.Collections.Generic;

    public class LucidUI : IStateful<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>
    {
        public IStateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>
            GetStateMachine(IStateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType> stateMachine)
        {
            return stateMachine;
        }

        public IEnumerable<SavedSearch> SavedSearches { get; set; }
    }
}