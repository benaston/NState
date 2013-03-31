using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NState
{
    public static class StateMachineSerializationHelper
    {
        public static string ToJson<TState, TTransitionActionStatus>(this IStateMachine<TState, TTransitionActionStatus> stateMachine) where TState : State
        {
            dynamic dto = SerializeToDto(stateMachine, new ExpandoObject());
            dynamic s = JsonConvert.SerializeObject(dto, Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        ObjectCreationHandling =
                                                            ObjectCreationHandling.Replace
                                                    });

            return s;
        }

        public static IStateMachine<TState, TTransitionActionStatus> InitializeFromJson<TState, TTransitionActionStatus>(this IStateMachine<TState, TTransitionActionStatus> stateMachine, string json) 
            where TState : State 
        {
            return InitializeWithDto(stateMachine, JsonConvert.DeserializeObject(json));
        }

        /// <summary>
        /// State machine to be initlialized must match the serialized DTO.
        /// </summary>
        private static IStateMachine<TState, TTransitionActionStatus> InitializeWithDto<TState, TTransitionActionStatus>(
            IStateMachine<TState, TTransitionActionStatus> stateMachine,
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
                    InitializeWithDto<TState, TTransitionActionStatus>(stateMachine.Children[c.Name], c.Value);
                }
            }

            return stateMachine;
        }

        private static dynamic SerializeToDto<TState, TTransitionActionStatus>(
            IStateMachine<TState, TTransitionActionStatus> node, ExpandoObject dto)
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