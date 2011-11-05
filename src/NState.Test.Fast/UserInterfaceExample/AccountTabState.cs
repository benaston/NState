namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class AccountTabState : LucidState
    {
        public class Hidden : AccountTabState {}

        public class Visible : AccountTabState {}
    }
}