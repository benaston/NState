namespace NState.Test.Fast.UserInterfaceExample
{
    public class AccountTabHelper
    {
        public static void Hide(LucidState state, dynamic args)
        {
            args.SearchTabSM.CurrentState = new SearchTabState.Visible();
        }

        public static void Show(LucidState state, dynamic args)
        {
            args.SearchTabSM.CurrentState = new SearchTabState.Hidden();
        }
    }
}