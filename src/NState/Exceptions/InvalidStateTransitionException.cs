using System;

namespace NState.Exceptions
{
    public class InvalidStateTransitionException<TState> : Exception
        where TState : State
    {
        public InvalidStateTransitionException(TState initialState,
                                               TState endState,
                                               Exception innerException = default(Exception))
            : base(string.Format(StringTable.InvalidStateTransitionExceptionMessage, initialState.Name, endState.Name), innerException) { }
    }
}