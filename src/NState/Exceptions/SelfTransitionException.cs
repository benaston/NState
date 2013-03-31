using System;

namespace NState.Exceptions
{
    public class SelfTransitionException : Exception
    {
        public SelfTransitionException() : base(StringTable.SelfTransitionExceptionMessage) { }
    }
}