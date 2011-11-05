namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchAreaHelper
    {
        public static void Show(LucidState state, dynamic args)
        {
            //when showing the search panel, ensure the working panel is in the home position?
            //       search panel search tab         home panel         root
            args.StateMachine.Parent.Parent.Parent.Children["WorkingPanel"].TriggerTransition(new WorkingPanelState.SearchMode());
            args.StateMachine.Parent.Parent.Parent.Children["DetailsPanels"].TriggerTransition(new DetailsPanelsState.SearchMode());
        }
    }
}