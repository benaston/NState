namespace NState.Test.Fast.UserInterfaceExample
{
    public class AccountTab : Stateful<UIRoot, LucidState>
    {
        public AccountTab(IStateMachine<LucidState> stateMachine)
            : base(stateMachine) {}

        public AccountTab Hide()
        {
            TriggerTransition(this, new AccountTabState.Hidden());

            return this;
        }

        public AccountTab Show()
        {
            TriggerTransition(this, new AccountTabState.Visible());

            return this;
        }
    }
}