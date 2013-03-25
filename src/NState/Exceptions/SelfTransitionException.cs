using System;

namespace NState.Exceptions
{
    public class SelfTransitionException : Exception
    {
        public const string MessageFormat = "Self transition not permitted. Your state machine has a constructor argument to enable self-transitions.";

        public SelfTransitionException() : base(MessageFormat) { }
    }
}