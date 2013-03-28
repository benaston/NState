// ReSharper disable InconsistentNaming
using NState.Exceptions;
using NState.Test.Fast.BugTrackerExample.TransitionActions;
using NState.Test.Fast.BugTrackerExample.Transitions;

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
            var transitions = new IStateTransition<BugState, TransitionStatus>[]
            {
                new BugTransition.Open(),
                new BugTransition.Assign(new BugTransitionAction.Assign()),
                new BugTransition.Defer(new BugTransitionAction.Defer()),
                new BugTransition.Resolve(new BugTransitionAction.Resolve()),
                new BugTransition.Close(new BugTransitionAction.Close()),
            };

            _stateMachine = new StateMachine<BugState, TransitionStatus>("Bug",
                                                                            transitions,
                                                                            initialState: new BugState.Open());
            _bug = new Bug("bug1");
        }

        private StateMachine<BugState, TransitionStatus> _stateMachine;
        private Bug _bug;


        [Test]
        public void TriggerTransition_IdentityTransition_NoExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionStatus transitionStatus;

            //act
            bug.Assign("assignee", out transitionStatus);

            //assert
            Assert.That(bug.CurrentState, Is.TypeOf<BugState.Assigned>());
            Assert.That(transitionStatus, Is.EqualTo(TransitionStatus.Success));
        }

        [Test]
        public void TriggerTransition_InvalidTransition_ExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionStatus transitionStatus;

            //act & assert
            Assert.Throws<InvalidStateTransitionException<BugState>>(() => bug.Resolve(out transitionStatus));
        }

        [Test]
        public void TriggerTransition_TwoSuccessiveValidTransitions_NoExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionStatus transitionStatus;

            //act
            bug.Assign("example@example.com", out transitionStatus)
               .Defer(out transitionStatus);

            //assert
            Assert.That(bug.CurrentState, Is.TypeOf<BugState.Deferred>());
            Assert.That(transitionStatus, Is.EqualTo(TransitionStatus.Success));
        }

        [Test]
        public void TriggerTransition_UnexpectedDtoSupplied_NoExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            dynamic args = new ExpandoObject();
            args.Blah = "blah";
            TransitionStatus transitionStatus;

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Deferred(), out transitionStatus, args));
        }

        [Test]
        public void TriggerTransition_ValidTransitionWithArgument_ArgumentSetInTargetObjectCorrectly()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            const string assigneeEmail = "example@example.com";
            TransitionStatus transitionStatus;

            //act
            bug.Assign(assigneeEmail, out transitionStatus);

            //assert
            Assert.That(bug.Bug.AssigneeEmail, Is.EqualTo(assigneeEmail));
        }

        [Test]
        public void TriggerTransition_ValidTransition_NoExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            dynamic args = new ExpandoObject();
            args.AssigneeEmail = "example@example.com";

            //act/assert
            TransitionStatus transitionStatus;
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Assigned(), out transitionStatus, args));
        }
    }
}

// ReSharper restore InconsistentNaming