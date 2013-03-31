using System;

namespace NState.Exceptions
{
    public class FinalStateTransitionException : Exception
    {
        public FinalStateTransitionException() : base(StringTable.FinalStateTransitionExceptionMessage) { }
    }
}