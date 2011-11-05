namespace NState.Test.Fast.UserInterfaceExample
{
    public class AccountTab : Stateful<UIRoot, LucidState>
    {
        public AccountTab(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) {}

        public AccountTab Hide()
        {
            TriggerTransition(this, new AccountTabState.Hidden(),
                              new
                                  {
                                      SearchTabSM = StateMachine.Parent.Children["SearchTab"],
                                      AccountTabSM = StateMachine,
                                  });

            return this;
        }

        public AccountTab Show()
        {
            TriggerTransition(this, new AccountTabState.Visible(),
                              new
                                  {
                                      SearchTabSM = StateMachine.Parent.Children["SearchTab"],
                                      AccountTabSM = StateMachine,
                                  });

            return this;
        }
    }
}