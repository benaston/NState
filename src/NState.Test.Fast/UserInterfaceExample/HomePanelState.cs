namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class HomePanelState : LucidState
    {
        public class Hidden : HomePanelState {}

        public class Visible : HomePanelState {}
    }
}