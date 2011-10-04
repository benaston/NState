namespace NState.Test.Fast
{
    public class LucidUITransitions
    {
        public class Pause : StateTransition<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>
        {
            public override LucidUIState StartState
            {
                get { return new LucidUIState.Active(); }
            }

            public override LucidUIState EndState
            {
                get { return new LucidUIState.Paused(); }

            }
        }

        public class Resume : StateTransition<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>
        {
            public override LucidUIState StartState
            {
                get { return new LucidUIState.Paused(); }
            }

            public override LucidUIState EndState
            {
                get { return new LucidUIState.Active(); }
            }
        }
    }
}