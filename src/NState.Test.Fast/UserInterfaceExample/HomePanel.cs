namespace NState.Test.Fast.UserInterfaceExample
{
    public class HomePanel : Stateful<UIRoot, LucidState>
    {
        public HomePanel(IStateMachine<LucidState> stateMachine)
            : base(stateMachine) { }

        public HomePanel Hide()
        {
            return TriggerTransition(this, new HomePanelState.Hidden());
        }

        public HomePanel Show()
        {
            return TriggerTransition(this, new HomePanelState.Visible());
        }
    }
}