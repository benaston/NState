namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class WorkingPanelState : LucidState
    {
        public class SearchMode : WorkingPanelState { }

        public class AccountMode : WorkingPanelState { }
    }
}