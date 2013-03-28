using System.Dynamic;
using NState.Test.Fast.BugTrackerExample;
using NState.Test.Fast.BugTrackerExample.TransitionActions;
using NState.Test.Fast.BugTrackerExample.Transitions;

namespace NState.Test.Fast.SerializationExample
{
    using NUnit.Framework;

    [TestFixture]
    public class SerializationExampleTests
    {
        private StateMachine<BugState, BugTransitionStatus> _stateMachine;
        private IStateTransition<BugState, BugTransitionStatus>[] _transitions;

        [SetUp]
        public void Setup()
        {
            _transitions = new IStateTransition<BugState, BugTransitionStatus>[]
            {
                new BugTransition.Assign(new BugTransitionAction.Assign()),
            };

            _stateMachine = new StateMachine<BugState, BugTransitionStatus>("example",
                                                                            _transitions,
                                                                            initialState: new BugState.Open());
        }


        [Test]
        public void DeserializeTest()
        {
            Assert.That(_stateMachine.CurrentState == new BugState.Open());
            dynamic statefulObject = new ExpandoObject();
            statefulObject.Bug = new Bug("foo");
            _stateMachine.TriggerTransition(new BugState.Assigned(), statefulObject, new { AssigneeEmail = "example@example.com" });
            Assert.That(_stateMachine.CurrentState == new BugState.Assigned());

            //arrange
            string json = _stateMachine.ToJson();

            _stateMachine = new StateMachine<BugState, BugTransitionStatus>("example",
                                                                            _transitions,
                                                                            initialState: new BugState.Open());

            Assert.That(_stateMachine.CurrentState == new BugState.Open());

            _stateMachine.InitializeFromJson(json);

            Assert.That(_stateMachine.CurrentState == new BugState.Assigned());
        }

        [Test]
        public void SerializeTest()
        {
            //arrange
            Assert.DoesNotThrow(() => _stateMachine.ToJson());
        }
    }
}

// ReSharper restore InconsistentNaming