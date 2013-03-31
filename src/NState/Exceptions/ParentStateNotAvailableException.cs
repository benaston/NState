using System;

namespace NState.Exceptions
{
    public class ParentStateNotAvailableException : Exception
    {
        public ParentStateNotAvailableException() : base(StringTable.ParentStateNotAvailableExceptionMessage) { }
    }
}