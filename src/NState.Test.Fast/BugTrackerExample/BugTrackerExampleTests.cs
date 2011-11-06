// ReSharper disable InconsistentNaming
namespace NState.Test.Fast.BugTrackerExample
{
    using System.Dynamic;
    using NUnit.Framework;

    [TestFixture]
    public class BugTrackerExampleTests
    {
        [SetUp]
        public void Setup()
        {
            var bugTransitions = new IStateTransition<BugState>[]
                                  {
                                      new BugTransition.Open(),
                                      new BugTransition.Assign(BugTransitionFunction.Assign),
                                      new BugTransition.Defer(BugTransitionFunction.Defer),
                                      new BugTransition.Resolve(BugTransitionFunction.Resolve),
                                      new BugTransition.Close(BugTransitionFunction.Close),
                                  };

            _stateMachine = new StateMachine<BugState>("Bug",
                                                            bugTransitions,
                                                            startState: new BugState.Open());
        }

        private StateMachine<BugState> _stateMachine;

        

        [Test]
        public void TriggerTransition_IdentityTransition_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.DoesNotThrow(() => bug.Open());
            Assert.That(bug.CurrentState == new BugState.Open());
        }

        [Test]
        public void TriggerTransition_InvalidTransition_ExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.Throws<InvalidStateTransitionException<BugState>>(() => bug.Resolve());
        }

        [Test]
        public void TriggerTransition_TwoSuccessiveValidTransitions_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.Assign("example@example.com").Defer());
            Assert.That(bug.CurrentState == new BugState.Deferred());
        }

        [Test]
        public void TriggerTransition_UnexpectedDtoSupplied_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            dynamic args = new ExpandoObject();
            args.Blah = "blah";

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Open(), args));
        }

        [Test]
        public void TriggerTransition_ValidTransitionWithArgument_ArgumentSetInTargetObjectCorrectly()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            const string assigneeEmail = "example@example.com";

            //act/assert
            bug.Assign(assigneeEmail);

            Assert.That(bug.AssigneeEmail == assigneeEmail);
        }

        [Test]
        public void TriggerTransition_ValidTransition_NoExceptionThrown()
        {
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