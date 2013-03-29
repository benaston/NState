using System;

namespace NState.Exceptions
{
    public class SelfTransitionException : Exception
    {
        public const string MessageFormat = "Self transition not permitted. Please use the relevant constructor parameter on your state machine to enable self-transitions.";

        public SelfTransitionException() : base(MessageFormat) { }
    }
}