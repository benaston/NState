namespace NState.Test.Fast
{
    public class AccountTabTransitions
    {
        public class Expand : StateTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
        {
            public override AccountTabState StartState
            {
                get { return new AccountTabState.Collapsed(); }
            }

            public override AccountTabState EndState
            {
                get { return new AccountTabState.Expanded(); }

            }
        }

        public class Collapse : StateTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
        {
            public override AccountTabState StartState
            {
                get { return new AccountTabState.Expanded(); }
            }

            public override AccountTabState EndState
            {
                get { return new AccountTabState.Collapsed(); }

            }
        }
    }
}