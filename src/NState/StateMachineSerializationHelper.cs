using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NState
{
    public class StateMachineSerializationHelper
    {
        /// <summary>
        /// State machine to be hydrated must match the serialized DTO. Static because of recursive nature.
        /// </summary>
        public static IStateMachine<TState, TTransitionStatus> InitializeWithDto<TState, TTransitionStatus>(
            IStateMachine<TState, TTransitionStatus> stateMachine,
            dynamic dtoNode)
            where TState : State
        {
            if (stateMachine == null)
            {
                throw new ArgumentNullException("stateMachine");
            }

            if (dtoNode == null)
            {
                throw new ArgumentNullException("dtoNode");
            }

            if (stateMachine.Name != (string)dtoNode.Name)
            {
                throw new Exception("Mismatch between serialized object source and target in memory object. Ensure you are hydrating a state machine of the same type used to create the serialized object source.");
            }

            stateMachine.CurrentState =
                (TState) Activator.CreateInstance(Type.GetType((string) dtoNode.CurrentState.Name));

            if (dtoNode.Children != null && ((JEnumerable<JToken>) dtoNode.Children).Any())
            {
                foreach (dynamic c in dtoNode.Children)
                {
                    InitializeWithDto<TState, TTransitionStatus>(stateMachine.Children[c.Name], c.Value);
                }
            }

            return stateMachine;
        }

        /// <summary>
        /// Static because of recursive nature.
        /// </summary>
        public static dynamic SerializeToDto<TState, TTransitionStatus>(IStateMachine<TState, TTransitionStatus> node, ExpandoObject dto)
            where TState : State
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (dto == null)
            {
                throw new ArgumentNullException("dto");
            }

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