namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class DetailsPanelsState : LucidState
    {
        public class SearchMode : DetailsPanelsState { }

        public class AccountMode : DetailsPanelsState { }
    }
}