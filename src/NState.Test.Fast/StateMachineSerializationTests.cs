// ReSharper disable InconsistentNaming

using System.Dynamic;
using NState.Test.Fast.BugTrackerExample;
using NState.Test.Fast.BugTrackerExample.TransitionActions;
using NState.Test.Fast.BugTrackerExample.Transitions;
using NUnit.Framework;

namespace NState.Test.Fast
{
    [TestFixture]
    public class StateMachineSerializationTests
    {
        private StateMachine<BugState, TransitionActionStatus> _stateMachine;
        private IStateTransition<BugState, TransitionActionStatus>[] _transitions;

        [SetUp]
        public void Setup()
        {
            _transitions = new IStateTransition<BugState, TransitionActionStatus>[]
            {
                new BugTransition.Assign(new BugTransitionAction.Assign()),
            };

            _stateMachine = new StateMachine<BugState, TransitionActionStatus>("example",
                                                                               _transitions,
                                                                               initialState: new BugState.Open());
        }

        [Test]
        public void InitializeFromJson_ValidJson_Succeeds()
        {
            //arrange
            dynamic statefulObject = new ExpandoObject();
            statefulObject.Bug = new Bug("foo");
            _stateMachine.TriggerTransition(new BugState.Assigned(), statefulObject, new { AssigneeEmail = "example@example.com" });
            var stateMachine2 = new StateMachine<BugState, TransitionActionStatus>("example",
                                                                                   _transitions,
                                                                                   initialState: new BugState.Open());

            //assert
            Assert.That(_stateMachine.CurrentState == new BugState.Assigned());
            Assert.That(stateMachine2.CurrentState == new BugState.Open());

            //act
            string json = _stateMachine.ToJson();
            _stateMachine.InitializeFromJson(json);

            //assert
            Assert.That(_stateMachine.CurrentState, Is.TypeOf<BugState.Assigned>());
        }
    }
}

// ReSharper restore InconsistentNaming