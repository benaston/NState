using System;

namespace NState.Exceptions
{
    public class InvalidStateTransitionException<TState> : Exception
        where TState : State
    {
        private const string DefaultMessage = "Unable to transition state from {0} to {1}.";

        public InvalidStateTransitionException(TState initialState,
                                               TState endState,
                                               Exception innerException = default(Exception))
            : base(string.Format(DefaultMessage, initialState.Name, endState.Name), innerException) { }
    }
}