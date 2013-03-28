using System;

namespace NState.Exceptions
{
    public class TransitionActionException : Exception
    {
        public TransitionActionException(string message) : base(message) { }
    }
}