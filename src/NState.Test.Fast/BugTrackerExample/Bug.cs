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

namespace NState.Test.Fast.BugTrackerExample
{
	using System.Dynamic;

	public class Bug : Stateful<Bug, BugState>
	{
		public Bug(string title, IStateMachine<BugState> stateMachine)
			: base(stateMachine) {
			Title = title;
		}

		public string Title { get; set; }

		public string AssigneeEmail { get; set; }

		public string ClosedByName { get; set; }

		public Bug Open() {
			return TriggerTransition(this, new BugState.Open());
		}

		public Bug Assign(string assigneeEmail) {
			dynamic args = new ExpandoObject();
			args.AssigneeEmail = assigneeEmail;

			return TriggerTransition(this, new BugState.Assigned(), args);
		}

		public Bug Defer() {
			return TriggerTransition(this, new BugState.Deferred());
		}

		public Bug Resolve() {
			return TriggerTransition(this, new BugState.Resolved());
		}

		public Bug Close(string closedByName) {
			dynamic args = new ExpandoObject();
			args.ClosedByName = closedByName;

			return TriggerTransition(this, new BugState.Closed(), args);
		}
	}
}