namespace NState.Test.Fast.UserInterfaceExample
{
    public class UIRoot : Stateful<UIRoot, LucidState>
    {
        public UIRoot(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) {}

        public UIRoot Hide()
        {
            return TriggerTransition(this, new HomePanelState.Hidden());
        }

        public UIRoot Show()
        {
            return TriggerTransition(this, new HomePanelState.Visible());
        }
    }
}