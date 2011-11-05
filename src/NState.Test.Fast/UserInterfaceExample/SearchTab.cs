namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchTab : Stateful<UIRoot, LucidState>
    {
        public SearchTab(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) {}

        public SearchTab Hide()
        {
            return TriggerTransition(this, new SearchTabState.Hidden(),
                                     new
                                         {
                                             SearchTabSM = StateMachine,
                                             AccountTabSM =
                                         StateMachine.Parent.Children["AccountTab"],
                                         });

            //return this;
        }

        public SearchTab Show()
        {
            return TriggerTransition(this, new SearchTabState.Visible(),
                                     new
                                         {
                                             SearchTabSM = StateMachine,
                                             AccountTabSM =
                                         StateMachine.Parent.Children["AccountTab"],
                                         });

            //return this;
        }
    }
}