namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class SavedSearchAreaState : LucidState
    {
        public class Hidden : SavedSearchAreaState { }

        public class Visible : SavedSearchAreaState { }
    }
}