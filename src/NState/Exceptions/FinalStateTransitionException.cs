using System;

namespace NState.Exceptions
{
    public class FinalStateTransitionException : Exception
    {
        public const string MessageFormat = "Transition not permitted because the state machine is in its final state.";

        public FinalStateTransitionException() : base(MessageFormat) { }
    }
}