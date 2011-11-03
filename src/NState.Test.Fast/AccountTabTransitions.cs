namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class AccountTabTransitions
    {
        [Serializable]
        public class Collapse : StateTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
        {
            public Collapse(Func<AccountTab, AccountTabState, AccountTab> transitionFunction) : base(transitionFunction) {}

            public override AccountTabState StartState
            {
                get { return new AccountTabState.Expanded(); }
            }

            public override AccountTabState EndState
            {
                get { return new AccountTabState.Collapsed(); }
            }
        }

        [Serializable]
        public class Expand : StateTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
        {
            public Expand(Func<AccountTab, AccountTabState, AccountTab> transitionFunction) : base(transitionFunction) {}

            public override AccountTabState StartState
            {
                get { return new AccountTabState.Collapsed(); }
            }

            public override AccountTabState EndState
            {
                get { return new AccountTabState.Expanded(); }
            }
        }
    }
}