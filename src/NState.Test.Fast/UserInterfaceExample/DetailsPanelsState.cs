namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class DetailsPanelsState : LucidState
    {
        public class AccountMode : DetailsPanelsState {}

        public class SearchMode : DetailsPanelsState {}
    }
}