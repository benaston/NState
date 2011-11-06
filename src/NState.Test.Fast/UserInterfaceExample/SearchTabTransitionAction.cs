namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchTabTransitionAction
    {
        public static void Hide(LucidState state, IStateMachine<LucidState> stateMachine, dynamic args)
        {
            stateMachine.Parent.Children["AccountTab"].CurrentState = new AccountTabState.Visible();
        }

        public static void Show(LucidState state, IStateMachine<LucidState> stateMachine, dynamic args)
        {
            stateMachine.Parent.Children["AccountTab"].CurrentState = new AccountTabState.Hidden();
        }
    }
}