namespace NState
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using NSure;
    using ArgumentException = NHelpfulException.FrameworkExceptions.ArgumentException;

    public class StateMachineSerializationHelper
    {
        /// <summary>
        ///   State machine to be hydrated must match the serialized DTO.
        ///   Static because of recursive nature.
        /// </summary>
        public static IStateMachine<TState> InitializeWithDto<TState>(
            IStateMachine<TState> stateMachine,
            dynamic dtoNode)
            where TState : State
        {
            Ensure.That<ArgumentException>(stateMachine.Name == (string) dtoNode.Name,
                                           "Mismatch between serialized object source and target in memory object.",
                                           new[]
                                               {
                                                   "Ensure you are hydrating a state machine of the same type used to create the serialized object source."
                                               });

            stateMachine.CurrentState =
                (TState) Activator.CreateInstance(Type.GetType((string) dtoNode.CurrentState.Name));

            if (dtoNode.Children != null && ((JEnumerable<JToken>) dtoNode.Children).Any())
            {
                foreach (var c in dtoNode.Children)
                {
                    InitializeWithDto<TState>(stateMachine.Children[c.Name], c.Value);
                }
            }

            return stateMachine;
        }

        /// <summary>
        ///   Static because of recursive nature.
        /// </summary>
        public static dynamic SerializeToDto<TState>(IStateMachine<TState> node, ExpandoObject dto)
            where TState : State
        {
            ((dynamic) dto).Name = node.Name;
            ((dynamic) dto).CurrentState = node.CurrentState;

            if (node.Children.Any())
            {
                var children = new List<object>();
                foreach (var c in node.Children)
                {
                    children.Add(SerializeToDto(c.Value, new ExpandoObject()));
                }

                ((dynamic) dto).Children = children;
            }

            return dto;
        }
    }
}