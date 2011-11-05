namespace NState.Test.Fast.UserInterfaceExample
{
    public class SearchArea : Stateful<UIRoot, LucidState>
    {
        public SearchArea(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public SearchArea Hide()
        {
            TriggerTransition(this, new SearchAreaState.Hidden(),
                              new
                                  {
                                      //       search panel search tab         home panel         root
                                      StateMachine, //todo fix this madness - auto update root by walking the tree
                                  });

            return this;
        }

        public SearchArea Show()
        {
            TriggerTransition(this, new SearchAreaState.Visible(),
                              new
                                  {
                                      StateMachine,
                                  });

            return this;
        }
    }
}