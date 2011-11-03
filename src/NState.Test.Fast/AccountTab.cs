namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class AccountTab : Stateful<AccountTab, AccountTabState, StateMachineType>
    {
        public AccountTab(
            IStateMachine<AccountTab, AccountTabState, StateMachineType> stateMachine)
            : base(stateMachine) {}
    }
}