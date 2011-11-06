namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class WorkingPanelState : LucidState
    {
        public class AccountMode : WorkingPanelState {}

        public class SearchMode : WorkingPanelState {}
    }
}