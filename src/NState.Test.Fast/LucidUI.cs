namespace NState.Test.Fast
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class LucidUI : Stateful<LucidUI, LucidUIState, StateMachineType>
    {
        public LucidUI(IStateMachine<LucidUI, LucidUIState, StateMachineType> stateMachine)
            : base(stateMachine) {}

        public AccountTab AccountTab { get; set; }

        public IEnumerable<SavedSearch> SavedSearches { get; set; }
    }
}