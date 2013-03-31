using System;
using System.Linq;
using NState.Exceptions;

namespace NState
{
    /// <summary>
    /// NOTE 1: http://cs.hubfs.net/blogs/hell_is_other_languages/archive/2008/01/16/4565.aspx.
    /// NOTE 2: Transitions could be in-memory transactionalised using the 
    /// memento pattern, or information could be sent to F# (see NOTE 1).
    /// </summary>
    public static class StateMachineTransitionHelper
    {
        public static TTransitionActionStatus 
            TriggerTransition<TState, TTransitionActionStatus>(StateMachine<TState, TTransitionActionStatus> stateMachine, 
                                                               TState targetState,
                                                               dynamic statefulObject,
                                                               dynamic args = default(dynamic)) where TState : State
        {
            var result = default(TTransitionActionStatus);
            if (targetState == null)
            {
                throw new ArgumentNullException("targetState");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (stateMachine.CurrentState == targetState && !stateMachine.PermitSelfTransition)
            {
                throw new SelfTransitionException();
            }

            if (stateMachine.CurrentState == stateMachine.FinalState)
            {
                throw new FinalStateTransitionException();
            }

            if ((stateMachine.CurrentState != targetState ||
                (stateMachine.CurrentState == targetState && !stateMachine.BypassTransitionBehaviorForSelfTransition)))
            {
                var matches = stateMachine.StateTransitions
                                          .Where(t => t.StartStates
                                                       .Any(s => s == stateMachine.CurrentState) && 
                                                       t.EndStates.Any(e => e == targetState)).ToList();
                var match = matches.Count > 0 ? matches[0] : null;
                if (match != null && match.Condition(targetState, statefulObject, args))
                {
                    stateMachine.CurrentState.ExitAction(args);
                    result = match.TransitionAction.Run(targetState, stateMachine, statefulObject, args);
                    targetState.EntryAction(args);
                    stateMachine.CurrentState = targetState;
                }
                else
                {
                    if (stateMachine.Parent == null)
                    {
                        throw new InvalidStateTransitionException<TState>(stateMachine.CurrentState,
                                                                targetState);
                    }

                    stateMachine.Parent.TriggerTransition(targetState, args);
                }
            }

            return result;
        }
    }
}