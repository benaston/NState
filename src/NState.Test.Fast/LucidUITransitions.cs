namespace NState.Test.Fast
{
    using System;

    public class LucidUITransitions
    {
        public class Pause : StateTransition<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>
        {
            public Pause(Func<LucidUI, LucidUIState, LucidUI> transitionFunction) : base(transitionFunction) {}

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
            public Resume(Func<LucidUI, LucidUIState, LucidUI> transitionFunction) : base(transitionFunction) {}

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