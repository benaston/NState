// Copyright 2012, Ben Aston (ben@bj.ma).
// 
// This file is part of NState.
// 
// NState is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NState is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NState. If not, see <http://www.gnu.org/licenses/>.

namespace NState
{
	using System;

	/// <summary>
	/// 	Defines the interface for a state transition for a type that supports "workflow". Examples of actions being performed during a transition include the sending of an email or SMS, the persisting of information to the database, or an onscreen notification or the billing of funds. Note that a computer program might invoke the state transition.
	/// </summary>
	public interface IStateTransition<TState>
		where TState : State
	{
		TState[] InitialStates { get; }

		TState[] EndStates { get; }

		/// <summary>
		/// 	A constraint which will fire the transition only when it is evaluated to true after the trigger occurs.
		/// </summary>
		Func<TState, dynamic, bool> Condition { get; }

		/// <summary>
		/// 	An action which is executed when performing a certain transition.
		/// </summary>
		Action<TState, IStateMachine<TState>, dynamic> TransitionAction { get; }
	}
}