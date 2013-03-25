using System;

namespace NState.Exceptions
{
    public class ParentStateNotAvailableException : Exception
    {
        public const string MessageFormat = "An attempt was made to determine the state of a parent state machine that does not exist.";

        public ParentStateNotAvailableException() : base(MessageFormat) { }
    }
}