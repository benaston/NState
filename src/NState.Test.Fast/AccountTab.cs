namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class AccountTab : Stateful<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
    {
        public AccountTab(
            IStateMachine<AccountTab, AccountTabState, LucidUIState, StateMachineType> stateMachine)
            : base(stateMachine) {}
    }
}