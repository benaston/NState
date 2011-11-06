namespace NState
{
    using System;
    using NHelpfulException;

    public class InvalidStateTransitionException<TState> : HelpfulException
        where TState : State
    {
        private const string DefaultMessage = "Unable to transition state from {0} to {1}.";

        public InvalidStateTransitionException(TState initialState, TState endState,
                                               string[] resolutionSuggestions = default (string[]),
                                               Exception innerException = default(Exception))
            : base(string.Format(DefaultMessage, initialState.Name, endState.Name), resolutionSuggestions, innerException) {}
    }
}