namespace NState.Test.Fast.UserInterfaceExample
{
    using System;

    public class AccountTabTransition
    {
        [Serializable]
        public class Hide : StateTransition<LucidState>
        {
            public Hide(Action<LucidState, IStateMachine<LucidState>, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] {new AccountTabState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new AccountTabState.Hidden(),}; }
            }
        }

        [Serializable]
        public class Show : StateTransition<LucidState>
        {
            public Show(Action<LucidState, IStateMachine<LucidState>, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] {new AccountTabState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new AccountTabState.Visible(),}; }
            }
        }
    }
}