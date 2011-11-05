namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchAreaTransitionFunction
    {
        /// <summary>
        /// //when showing the search panel, ensure the working panel is in the home position?
        /// </summary>
        public static void Show(LucidState state, IStateMachine<LucidState> stateMachine, dynamic args)
        {
            //       search panel search tab         home panel         root
            stateMachine.Parent.Parent.Parent.Children["WorkingPanel"].TriggerTransition(new WorkingPanelState.SearchMode());
            stateMachine.Parent.Parent.Parent.Children["DetailsPanels"].TriggerTransition(new DetailsPanelsState.SearchMode());
        }
    }
}