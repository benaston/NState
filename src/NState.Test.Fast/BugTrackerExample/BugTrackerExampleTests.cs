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

// ReSharper disable InconsistentNaming

namespace NState.Test.Fast.BugTrackerExample
{
	using System.Dynamic;
	using NUnit.Framework;

	[TestFixture]
	public class BugTrackerExampleTests
	{
		#region Setup/Teardown

		[SetUp]
		public void Setup() {
			var bugTransitions = new IStateTransition<BugState>[] {
				new BugTransition.Open(),
				new BugTransition.Assign(BugTransitionAction.Assign),
				new BugTransition.Defer(BugTransitionAction.Defer),
				new BugTransition.Resolve(BugTransitionAction.Resolve),
				new BugTransition.Close(BugTransitionAction.Close),
			};

			_stateMachine = new StateMachine<BugState>("Bug",
			                                           bugTransitions,
			                                           initialState: new BugState.Open());
		}

		#endregion

		private StateMachine<BugState> _stateMachine;


		[Test]
		public void TriggerTransition_IdentityTransition_NoExceptionThrown() {
			//arrange
			var bug = new Bug("bug1", _stateMachine);

			//act/assert
			Assert.That(bug.CurrentState == new BugState.Open());
			Assert.DoesNotThrow(() => bug.Open());
			Assert.That(bug.CurrentState == new BugState.Open());
		}

		[Test]
		public void TriggerTransition_InvalidTransition_ExceptionThrown() {
			//arrange
			var bug = new Bug("bug1", _stateMachine);

			//act/assert
			Assert.Throws<InvalidStateTransitionException<BugState>>(() => bug.Resolve());
		}

		[Test]
		public void TriggerTransition_TwoSuccessiveValidTransitions_NoExceptionThrown() {
			//arrange
			var bug = new Bug("bug1", _stateMachine);

			//act/assert
			Assert.DoesNotThrow(() => bug.Assign("example@example.com").Defer());
			Assert.That(bug.CurrentState == new BugState.Deferred());
		}

		[Test]
		public void TriggerTransition_UnexpectedDtoSupplied_NoExceptionThrown() {
			//arrange
			var bug = new Bug("bug1", _stateMachine);
			dynamic args = new ExpandoObject();
			args.Blah = "blah";

			//act/assert
			Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Open(), args));
		}

		[Test]
		public void TriggerTransition_ValidTransitionWithArgument_ArgumentSetInTargetObjectCorrectly() {
			//arrange
			var bug = new Bug("bug1", _stateMachine);
			const string assigneeEmail = "example@example.com";

			//act/assert
			bug.Assign(assigneeEmail);

			Assert.That(bug.AssigneeEmail == assigneeEmail);
		}

		[Test]
		public void TriggerTransition_ValidTransition_NoExceptionThrown() {
			//arrange
			var bug = new Bug("bug1", _stateMachine);
			dynamic args = new ExpandoObject();
			args.AssigneeEmail = "example@example.com";

			//act/assert
			Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Assigned(), args));
		}
	}
}

// ReSharper restore InconsistentNaming