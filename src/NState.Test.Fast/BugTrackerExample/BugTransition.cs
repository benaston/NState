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

namespace NState.Test.Fast.BugTrackerExample
{
	using System;

	[Serializable]
	public class BugTransition
	{
		[Serializable]
		public class Assign : StateTransition<BugState>
		{
			public Assign(Action<BugState, IStateMachine<BugState>, object> transitionAction = null)
				: base(transitionAction) {}

			public override BugState[] InitialStates
			{
				get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
			}

			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Assigned(),}; }
			}
		}

		[Serializable]
		public class Close : StateTransition<BugState>
		{
			public Close(Action<BugState, IStateMachine<BugState>, object> transitionAction = null)
				: base(transitionAction) {}

			public override BugState[] InitialStates
			{
				get { return new BugState[] {new BugState.Resolved()}; }
			}

			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Closed(),}; }
			}
		}

		[Serializable]
		public class Defer : StateTransition<BugState>
		{
			public Defer(Action<BugState, IStateMachine<BugState>, object> transitionAction = null)
				: base(transitionAction) {}

			public override BugState[] InitialStates
			{
				get { return new BugState[] {new BugState.Open(), new BugState.Assigned()}; }
			}

			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Deferred(),}; }
			}
		}

		[Serializable]
		public class Open : StateTransition<BugState>
		{
			public Open(Action<BugState, IStateMachine<BugState>, object> transitionAction = null)
				: base(transitionAction) {}

			public override BugState[] InitialStates
			{
				get { return new[] {new BugState.Closed(),}; }
			}

			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Open(),}; }
			}
		}

		[Serializable]
		public class Resolve : StateTransition<BugState>
		{
			public Resolve(Action<BugState, IStateMachine<BugState>, object> transitionAction = null)
				: base(transitionAction) {}

			public override BugState[] InitialStates
			{
				get { return new[] {new BugState.Assigned(),}; }
			}

			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Resolved(),}; }
			}
		}
	}
}