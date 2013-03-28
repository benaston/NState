﻿// ReSharper disable InconsistentNaming
using NState.Exceptions;

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
            var bugTransitions = new IStateTransition<BugState, BugTransitionStatus>[]
            {
                new BugTransition.Open(),
                new BugTransition.Assign(new BugTransitionAction.Assign()),
                new BugTransition.Defer(new BugTransitionAction.Defer()),
                new BugTransition.Resolve(new BugTransitionAction.Resolve()),
                new BugTransition.Close(new BugTransitionAction.Close()),
            };

            _stateMachine = new StateMachine<BugState, BugTransitionStatus>("Bug",
                                                                            bugTransitions,
                                                                            initialState: new BugState.Open());
        }

        private StateMachine<BugState, BugTransitionStatus> _stateMachine;


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
            BugTransitionStatus transitionStatus;
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Open(), out transitionStatus, args));
        }

        [Test]
        public void TriggerTransition_ValidTransitionWithArgument_ArgumentSetInTargetObjectCorrectly()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            const string assigneeEmail = "example@example.com";

            //act
            bug.Assign(assigneeEmail);

            //assert
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
            BugTransitionStatus transitionStatus;
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Assigned(), out transitionStatus, args));
        }
    }
}

// ReSharper restore InconsistentNaming