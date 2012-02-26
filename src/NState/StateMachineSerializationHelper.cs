// Copyright 2011, Ben Aston (ben@bj.ma.)
// 
// This file is part of NState.
// 
// NFeature is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NFeature is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NState.  If not, see <http://www.gnu.org/licenses/>.

namespace NState
{
	using System;
	using System.Collections.Generic;
	using System.Dynamic;
	using System.Linq;
	using NBasicExtensionMethod;
	using NSure;
	using Newtonsoft.Json.Linq;
	using ArgumentException = NHelpfulException.FrameworkExceptions.ArgumentException;

	public class StateMachineSerializationHelper
	{
		/// <summary>
		/// 	State machine to be hydrated must match the serialized DTO. Static because of recursive nature.
		/// </summary>
		public static IStateMachine<TState> InitializeWithDto<TState>(
			IStateMachine<TState> stateMachine,
			dynamic dtoNode)
			where TState : State {
			Ensure.That<ArgumentException>(stateMachine.IsNotNull(), "stateMachine not supplied")
				.And<ArgumentException>(dtoNode != null, "dtoNode notSupplied")
				.And<ArgumentException>(stateMachine.Name == (string) dtoNode.Name,
				                        "Mismatch between serialized object source and target in memory object.",
				                        new[] {
				                        	"Ensure you are hydrating a state machine of the same type used to create the serialized object source."
				                        });

			stateMachine.CurrentState =
				(TState) Activator.CreateInstance(Type.GetType((string) dtoNode.CurrentState.Name));

			if (dtoNode.Children != null && ((JEnumerable<JToken>) dtoNode.Children).Any()) {
				foreach (dynamic c in dtoNode.Children) {
					InitializeWithDto<TState>(stateMachine.Children[c.Name], c.Value);
				}
			}

			return stateMachine;
		}

		/// <summary>
		/// 	Static because of recursive nature.
		/// </summary>
		public static dynamic SerializeToDto<TState>(IStateMachine<TState> node, ExpandoObject dto)
			where TState : State {
			Ensure.That<ArgumentException>(node.IsNotNull(), "node not supplied")
				.And<ArgumentException>(dto.IsNotNull(), "dto notSupplied");

			((dynamic) dto).Name = node.Name;
			((dynamic) dto).CurrentState = node.CurrentState;

			if (node.Children.Any()) {
				var children = new List<object>();
				foreach (var c in node.Children) {
					children.Add(SerializeToDto(c.Value, new ExpandoObject()));
				}

				((dynamic) dto).Children = children;
			}

			return dto;
		}
	}
}