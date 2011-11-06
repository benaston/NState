namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class SearchAreaState : LucidState
    {
        public class Hidden : SearchAreaState {}

        public class Visible : SearchAreaState {}
    }
}