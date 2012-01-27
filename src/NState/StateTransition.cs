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

	public abstract class StateTransition<TState> :
		IStateTransition<TState>
		where TState : State
	{
		protected StateTransition(
			Action<TState, IStateMachine<TState>, dynamic> transitionAction = null,
			Func<TState, dynamic, bool> condition = null)
		{
			Condition = condition ?? ((s, args) => true);
			TransitionAction = transitionAction ?? ((s, sm, args) => { });
		}

		public Func<TState, dynamic, bool> Condition { get; private set; }

		public Action<TState, IStateMachine<TState>, dynamic> TransitionAction { get; private set; }

		public abstract TState[] InitialStates { get; }

		public abstract TState[] EndStates { get; }
	}
}