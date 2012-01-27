// Copyright 2011, Ben Aston (ben@bj.ma.)
// 
// This file is part of NFeature.
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
// along with NFeature.  If not, see <http://www.gnu.org/licenses/>.

namespace NState
{
	using System;
	using System.Collections.Generic;

	public interface IStateMachine {}

	/// <summary>
	/// 	Responsible for defining the interface for types that control the transitions between state machine states.
	/// </summary>
	public interface IStateMachine<TState> : IStateMachine
		where TState : State
	{
		string Name { get; set; }

		IEnumerable<IStateTransition<TState>> StateTransitions { get; }

		TState InitialState { get; set; }

		IStateMachine<TState> Parent { get; set; }

		/// <summary>
		/// 	Key is SM name.
		/// </summary>
		Dictionary<string, IStateMachine<TState>> Children { get; set; }

		TState CurrentState { get; set; }

		Dictionary<DateTime, IStateTransition<TState>> History { get; set; }

		void TriggerTransition(TState targetState, dynamic dto = default(dynamic));
	}
}