namespace NState.Test.Fast.UserInterfaceExample
{
    public class DetailsPanels : Stateful<UIRoot, LucidState>
    {
        public DetailsPanels(IStateMachine<LucidState> stateMachine)
            : base(stateMachine) {}

        public DetailsPanels SelectSearchMode()
        {
            TriggerTransition(this, new DetailsPanelsState.SearchMode());

            return this;
        }

        public DetailsPanels SelectAccountMode()
        {
            TriggerTransition(this, new DetailsPanelsState.AccountMode());

            return this;
        }
    }
}