namespace NState.Test.Fast.UserInterfaceExample
{
    public class WorkingPanel : Stateful<UIRoot, LucidState>
    {
        public WorkingPanel(IStateMachine<LucidState> stateMachine)
            : base(stateMachine) {}

        public WorkingPanel SelectSearchMode()
        {
            TriggerTransition(this, new WorkingPanelState.SearchMode());

            return this;
        }

        public WorkingPanel SelectAccountMode()
        {
            TriggerTransition(this, new WorkingPanelState.AccountMode());

            return this;
        }
    }
}