namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchTab : Stateful<UIRoot, LucidState>
    {
        public SearchTab(IStateMachine<LucidState> stateMachine)
            : base(stateMachine) {}

        public SearchTab Hide()
        {
            return TriggerTransition(this, new SearchTabState.Hidden());
        }

        public SearchTab Show()
        {
            return TriggerTransition(this, new SearchTabState.Visible());
        }
    }
}