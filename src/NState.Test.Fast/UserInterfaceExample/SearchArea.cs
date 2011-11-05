namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchArea : Stateful<UIRoot, LucidState>
    {
        public SearchArea(IStateMachine<LucidState> stateMachine)
            : base(stateMachine) { }

        public SearchArea Hide()
        {
            TriggerTransition(this, new SearchAreaState.Hidden());

            return this;
        }

        public SearchArea Show()
        {
            TriggerTransition(this, new SearchAreaState.Visible());

            return this;
        }
    }
}