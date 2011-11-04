//namespace NState.Test.Fast
//{
//    using System;

//    [Serializable]
//    public class LucidUITransitions
//    {
//        [Serializable]
//        public class Pause : StateTransition<LucidUI, LucidUIState>
//        {
//            public Pause(Func<LucidUI, LucidUIState, dynamic, LucidUI> transitionFunction) : base(transitionFunction) {}

//            public override LucidUIState[] StartStates
//            {
//                get { return new [] { new LucidUIState.Active(), }; }
//            }

//            public override LucidUIState[] EndStates
//            {
//                get { return new[] { new LucidUIState.Paused(), }; }
//            }
//        }

//        [Serializable]
//        public class Resume : StateTransition<LucidUI, LucidUIState>
//        {
//            public Resume(Func<LucidUI, LucidUIState, dynamic, LucidUI> transitionFunction) : base(transitionFunction) {}

//            public override LucidUIState[] StartStates
//            {
//                get { return new[] { new LucidUIState.Paused(), }; }
//            }

//            public override LucidUIState[] EndStates
//            {
//                get { return new[] { new LucidUIState.Active(), }; }
//            }
//        }
//    }
//}