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

namespace NState.Test.Fast.SerializationExample
{
	using NUnit.Framework;
	using UserInterfaceExample;

	public abstract class SmState : LucidState
	{
		public class Hidden : SmState {}

		public class Visible : SmState {}
	}

	public class SmTransition
	{
		public class Hide : StateTransition<LucidState>
		{
			public override LucidState[] InitialStates {
				get { return new[] {new SmState.Visible(),}; }
			}

			public override LucidState[] EndStates {
				get { return new[] {new SmState.Hidden(),}; }
			}
		}

		public class Show : StateTransition<LucidState>
		{
			public override LucidState[] InitialStates {
				get { return new[] {new SmState.Hidden(),}; }
			}

			public override LucidState[] EndStates {
				get { return new[] {new SmState.Visible(),}; }
			}
		}
	}

	[TestFixture]
	public class SerializationExampleTests
	{
		[SetUp]
		public void Setup() {
			_stateMachineRoot = new StateMachine<LucidState>("Root",
			                                                 new IStateTransition<LucidState>[0],
			                                                 initialState: new UIRootState.Enabled());

			_transitions = new IStateTransition<LucidState>[] {
				new SmTransition.Hide(),
				new SmTransition.Show(),
			};

			_stateMachine1 = new StateMachine<LucidState>("SM1",
			                                              _transitions,
			                                              initialState: new SmState.Visible(),
			                                              parentStateMachine: _stateMachineRoot);
		}

		private StateMachine<LucidState> _stateMachine1;
		private StateMachine<LucidState> _stateMachineRoot;
		private IStateTransition<LucidState>[] _transitions;


		[Test]
		public void DeserializeTest() {
			//avoid name clashes when setting parents later in test
			var rootSM2 = new StateMachine<LucidState>("Root",
			                                           new IStateTransition<LucidState>[0],
			                                           initialState: new UIRootState.Enabled());

			Assert.That(_stateMachine1.CurrentState == new SmState.Visible());

			_stateMachine1.TriggerTransition(new SmState.Hidden());

			Assert.That(_stateMachine1.CurrentState == new SmState.Hidden());

			//arrange
			string json = _stateMachine1.ToJson();

			var sm2 = new StateMachine<LucidState>("SM1",
			                                       _transitions,
			                                       initialState: new SmState.Visible(),
			                                       parentStateMachine: rootSM2);

			Assert.That(sm2.CurrentState == new SmState.Visible());

			sm2.InitializeFromJson(json);

			Assert.That(sm2.CurrentState == new SmState.Hidden());
		}

		[Test]
		public void SerializeTest() {
			//arrange
			Assert.DoesNotThrow(() => _stateMachineRoot.ToJson());
		}
	}
}

// ReSharper restore InconsistentNaming