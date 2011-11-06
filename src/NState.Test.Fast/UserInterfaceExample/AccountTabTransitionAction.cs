namespace NState.Test.Fast.UserInterfaceExample
{
    public class AccountTabTransitionAction
    {
        public static void Hide(LucidState state, IStateMachine<LucidState> stateMachine, dynamic args)
        {
            stateMachine.Parent.Children["SearchTab"].CurrentState = new SearchTabState.Visible();
        }

        public static void Show(LucidState state, IStateMachine<LucidState> stateMachine, dynamic args)
        {
            stateMachine.Parent.Children["SearchTab"].CurrentState = new SearchTabState.Hidden();
        }
    }
}