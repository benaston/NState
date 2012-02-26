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
	using System.Dynamic;
	using NSure;
	using ArgumentException = NHelpfulException.FrameworkExceptions.ArgumentException;

	/// <summary>
	/// 	Inherit from this if you want to make your type stateful.
	/// </summary>
	public abstract class Stateful<TStatefulObject, TState> :
		IStateful<TStatefulObject, TState>
		where TStatefulObject : Stateful<TStatefulObject, TState>
		where TState : State
	{
		protected Stateful(
			IStateMachine<TState> stateMachine) {
			StateMachine = stateMachine;
		}

		public TState ParentState {
			get { return StateMachine.Parent.CurrentState; }
		}

		public IStateMachine<TState> StateMachine { get; set; }

		public TState CurrentState {
			get { return StateMachine.CurrentState; }
		}

		public TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject,
		                                                          TState targetState,
		                                                          ExpandoObject dto =
		                                                          	default(ExpandoObject)) {
			Ensure.That<ArgumentException>(statefulObject is ValueType ? true : statefulObject != null,
			                               "statefulObject not supplied.")
				.And<ArgumentException>(targetState != null, "targetState not supplied.");

			dto = dto ?? new ExpandoObject();
			((dynamic) dto).StatefulObject = statefulObject;
			StateMachine.TriggerTransition(targetState, dto);

			return statefulObject;
		}
	}
}