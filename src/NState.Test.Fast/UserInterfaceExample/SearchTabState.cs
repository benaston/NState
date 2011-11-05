namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class SearchTabState : LucidState
    {
        public class Hidden : SearchTabState {}

        public class Visible : SearchTabState {}
    }
}