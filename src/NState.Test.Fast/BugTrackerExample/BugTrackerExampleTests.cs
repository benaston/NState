// ReSharper disable InconsistentNaming
namespace NState.Test.Fast.BugTrackerExample
{
    using NUnit.Framework;

    [TestFixture]
    public class BugTrackerExampleTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            var bugTransitions = new IStateTransition<BugState>[]
                                  {
                                      new BugTransition.Open(),
                                      new BugTransition.Assign(BugHelper.Assign),
                                      new BugTransition.Defer(BugHelper.Defer),
                                      new BugTransition.Resolve(BugHelper.Resolve),
                                      new BugTransition.Close(BugHelper.Close),
                                  };

            _stateMachine = new StateMachine<Bug, BugState>("Bug",
                                                                     bugTransitions,
                                                                     startState: new BugState.Open());
        }

        #endregion

        private StateMachine<Bug, BugState> _stateMachine;

        [Test]
        public void CurrentState_ImmediatelyAfterConstruction_IsSetToInitialState()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.That(bug.StateMachine.Parent.CurrentState == new BugTrackerState.Extinguished());
        }

        [Test]
        public void TriggerTransition_IdentityTransition_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Open()));
            Assert.That(bug.CurrentState == new BugState.Open());
        }

        [Test]
        public void TriggerTransition_InvalidTransition_ExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.Throws<InvalidStateTransitionException<MyAppState>>(
                () => bug.TriggerTransition(bug, new BugState.Resolved()));
        }

        [Test]
        public void TriggerTransition_TwoSuccessiveValidTransitions_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Assigned(),
                                                            new
                                                                {
                                                                    StatefulObject = bug,
                                                                    AssigneeEmail = "example@example.com"
                                                                })
                                          .TriggerTransition(bug, new BugState.Deferred(), new {StatefulObject = bug}));
        }

        [Test]
        public void TriggerTransition_UnexpectedDtoSupplied_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Open(), new {Blah = "blah",}));
        }

        [Test]
        public void TriggerTransition_ValidTransitionWithArgument_ArgumentSetInTargetObjectCorrectly()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            const string assigneeEmail = "example@example.com";

            //act/assert
            bug = bug.TriggerTransition(bug, new BugState.Assigned(),
                                        new
                                            {
                                                StatefulObject = bug,
                                                AssigneeEmail = assigneeEmail
                                            });

            Assert.That(bug.AssigneeEmail == assigneeEmail);
        }

        [Test]
        public void TriggerTransition_ValidTransition_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Assigned(),
                                                            new
                                                                {
                                                                    StatefulObject = bug,
                                                                    AssigneeEmail = "example@example.com"
                                                                }));
        }
    }
}
// ReSharper restore InconsistentNaming