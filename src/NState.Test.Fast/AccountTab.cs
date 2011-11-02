namespace NState.Test.Fast
{
    public class AccountTab : IStateful<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
    {
        public IStateMachine<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType> GetStateMachineFromRootComposite(
            IStateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType> stateMachine)
        {
            return
                ((IStateMachine<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>)
                 (stateMachine.ChildStateMachines[StateMachineType.AccountTab]));
        }
    }
}