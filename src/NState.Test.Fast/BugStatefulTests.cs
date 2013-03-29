// ReSharper disable InconsistentNaming

using System.Dynamic;
using NState.Exceptions;
using NState.Test.Fast.BugTrackerExample;
using NState.Test.Fast.BugTrackerExample.TransitionActions;
using NState.Test.Fast.BugTrackerExample.Transitions;
using NUnit.Framework;

namespace NState.Test.Fast
{
    /// <summary>
    /// NOTE: these tests are *far* from comprehensive, 
    /// but will help with understanding how to use NState.
    /// </summary>
    [TestFixture]
    public class BugStatefulTests
    {
        private StateMachine<BugState, TransitionActionStatus> _stateMachine;
        private Bug _bug;
        private IStateTransition<BugState, TransitionActionStatus>[] _transitions;

        [SetUp]
        public void Setup()
        {
            _transitions = new IStateTransition<BugState, TransitionActionStatus>[]
                               {
                                   new BugTransition.Open(),
                                   new BugTransition.Assign(new BugTransitionAction.Assign()),
                                   new BugTransition.Defer(new BugTransitionAction.Defer()),
                                   new BugTransition.Resolve(new BugTransitionAction.Resolve()),
                                   new BugTransition.Close(new BugTransitionAction.Close()),
                               };

            _stateMachine = new StateMachine<BugState, TransitionActionStatus>("state machine name",
                                                                               _transitions,
                                                                               initialState: new BugState.Open());
            _bug = new Bug("bug1");
        }

        [Test]
        public void Assign_BugIsOpen_TransitionSuccess()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionActionStatus transitionActionStatus;
            const string assigneeEmail = "assignee@example.com";

            //act
            bug.Assign(assigneeEmail, out transitionActionStatus);

            //assert
            Assert.That(bug.CurrentState, Is.TypeOf<BugState.Assigned>());
            Assert.That(bug.Bug.AssigneeEmail, Is.EqualTo(assigneeEmail));
            Assert.That(transitionActionStatus, Is.EqualTo(TransitionActionStatus.Success));
        }

        [Test]
        public void Assign_BugIsAlreadyAssigned_TransitionSuccess()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionActionStatus transitionActionStatus;
            const string assigneeEmail1 = "assignee1@example.com";
            const string assigneeEmail2 = "assignee2@example.com";

            //act
            bug.Assign(assigneeEmail1, out transitionActionStatus);
            bug.Assign(assigneeEmail2, out transitionActionStatus);

            //assert
            Assert.That(bug.CurrentState, Is.TypeOf<BugState.Assigned>());
            Assert.That(bug.Bug.AssigneeEmail, Is.EqualTo(assigneeEmail2));
            Assert.That(transitionActionStatus, Is.EqualTo(TransitionActionStatus.Success));
        }

        [Test]
        public void Assign_BugIsAlreadyAssignedAndSelfTransitionNotPermitted_TransitionFails()
        {
            //arrange
            _stateMachine = new StateMachine<BugState, TransitionActionStatus>("state machine name",
                                                                               _transitions,
                                                                               permitSelfTransition: false,
                                                                               initialState: new BugState.Open());
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionActionStatus transitionActionStatus;
            const string assigneeEmail1 = "assignee1@example.com";
            const string assigneeEmail2 = "assignee2@example.com";

            //act
            bug.Assign(assigneeEmail1, out transitionActionStatus);

            var exceptionCaught = false;
            try
            {
                bug.Assign(assigneeEmail2, out transitionActionStatus);
            }
            catch (SelfTransitionException)
            {
                exceptionCaught = true;
            }

            //assert
            Assert.That(exceptionCaught, Is.EqualTo(true));
            Assert.That(bug.CurrentState, Is.TypeOf<BugState.Assigned>());
            Assert.That(bug.Bug.AssigneeEmail, Is.EqualTo(assigneeEmail1));
            Assert.That(transitionActionStatus, Is.EqualTo(TransitionActionStatus.Failed));
        }

        [Test]
        public void Resolve_BugIsOpen_ExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionActionStatus transitionActionStatus;

            //act & assert
            Assert.Throws<InvalidStateTransitionException<BugState>>(() => bug.Resolve(out transitionActionStatus));
        }

        [Test]
        public void AssignFollowedByDefer_BugIsOpen_TransitionsSucceed()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            TransitionActionStatus transitionActionStatus;

            //act
            bug.Assign("example@example.com", out transitionActionStatus)
               .Defer(out transitionActionStatus);

            //assert
            Assert.That(bug.CurrentState, Is.TypeOf<BugState.Deferred>());
            Assert.That(transitionActionStatus, Is.EqualTo(TransitionActionStatus.Success));
            Assert.That(bug.Bug.AssigneeEmail, Is.EqualTo(string.Empty));
        }

        [Test]
        public void TriggerTransition_UnexpectedDtoSupplied_NoExceptionThrown()
        {
            //arrange
            var bug = new BugStateful(_bug, _stateMachine);
            dynamic args = new ExpandoObject();
            args.Foo = "foo";
            TransitionActionStatus transitionActionStatus;

            //act & assert
            Assert.DoesNotThrow(() => bug.TriggerTransition(bug, new BugState.Deferred(), out transitionActionStatus, args));
        }
    }
}

// ReSharper restore InconsistentNaming