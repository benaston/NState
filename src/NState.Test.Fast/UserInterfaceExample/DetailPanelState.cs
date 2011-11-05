namespace NState.Test.Fast.UserInterfaceExample
{
    public abstract class DetailPanelState : LucidState
    {
        public class Hidden : DetailPanelState { }

        public class SideA : DetailPanelState { }

        public class SideB : DetailPanelState { }
    }
}