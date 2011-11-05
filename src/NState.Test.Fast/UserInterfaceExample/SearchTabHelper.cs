namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchTabHelper
    {
        public static void Hide(LucidState state, dynamic args)
        {
            args.AccountTabSM.CurrentState = new AccountTabState.Visible();
        }

        public static void Show(LucidState state, dynamic args)
        {
            args.AccountTabSM.CurrentState = new AccountTabState.Hidden();
        }
    }
}