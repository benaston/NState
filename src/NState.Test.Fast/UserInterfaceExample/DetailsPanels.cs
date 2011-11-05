namespace NState.Test.Fast.UserInterfaceExample
{
    public class DetailsPanels : Stateful<UIRoot, LucidState>
    {
        public DetailsPanels(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public DetailsPanels SelectSearchMode()
        {
            TriggerTransition(this, new DetailsPanelsState.SearchMode(),
                              new
                                  {
                                      StateMachine,
                                  });

            return this;
        }

        public DetailsPanels SelectAccountMode()
        {
            TriggerTransition(this, new DetailsPanelsState.AccountMode(),
                              new
                                  {
                                      StateMachine,
                                  });

            return this;
        }
    }
}