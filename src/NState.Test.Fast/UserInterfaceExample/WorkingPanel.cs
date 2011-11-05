namespace NState.Test.Fast.UserInterfaceExample
{
    public class WorkingPanel : Stateful<UIRoot, LucidState>
    {
        public WorkingPanel(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public WorkingPanel SelectSearchMode()
        {
            TriggerTransition(this, new WorkingPanelState.SearchMode(),
                              new
                                  {
                                      StateMachine, //todo fix this madness - auto update root by walking the tree
                                  });

            return this;
        }

        public WorkingPanel SelectAccountMode()
        {
            TriggerTransition(this, new WorkingPanelState.AccountMode(),
                              new
                                  {
                                      StateMachine,
                                  });

            return this;
        }
    }
}