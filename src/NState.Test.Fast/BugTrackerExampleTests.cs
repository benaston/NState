// ReSharper disable InconsistentNaming
namespace NState.Test.Fast
{
    using System;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using NUnit.Framework;

    public abstract class MyAppState : State { }

    public abstract class MyUnrelatedState : State
    {
        public class Extinguished : BugTrackerState { }

        public class Smoking : BugTrackerState { }
    }

    public abstract class MyOtherBugTrackerState : MyAppState
    {
        public class Extinguished : BugTrackerState { }

        public class Smoking : BugTrackerState { }
    }

    public abstract class BugTrackerState : MyAppState
    {
        public class Extinguished : BugTrackerState { }

        public class Smoking : BugTrackerState { }
    }

    [Serializable]
    public abstract class BugState : MyAppState
    {
        public class Assigned : BugState { }

        public class Closed : BugState { }

        public class Deferred : BugState { }

        public class Open : BugState { }

        public class Resolved : BugState { }
    }

    public class BugTracker : Stateful<BugTracker, MyAppState>
    {
        public BugTracker(IStateMachine<BugTracker, MyAppState> stateMachine)
            : base(stateMachine)
        {
        }
    }

    public class Bug : Stateful<BugTracker, MyAppState>
    {
        public Bug(string title, IStateMachine<BugTracker, MyAppState> stateMachine)
            : base(stateMachine)
        {
            Title = title;
        }

        public string Title { get; set; }

        public string AssigneeEmail { get; set; }

        public string ClosedByName { get; set; }

        public void Assign(string assigneeEmail)
        {
            TriggerTransition<Bug>(this, new BugState.Assigned(), new { AssigneeEmail = assigneeEmail });
        }

        public void Defer()
        {
            TriggerTransition<Bug>(this, new BugState.Deferred());
        }

        public void Resolve()
        {
            TriggerTransition<Bug>(this, new BugState.Resolved());
        }

        public void Close(string closedByName)
        {
            TriggerTransition<Bug>(this, new BugState.Closed(), new { ClosedByName = closedByName });
        }
    }

    public class BugTrackerTransition
    {
        [Serializable]
        public class SetAlight : StateTransition<BugTracker, MyAppState>
        {
            public SetAlight(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new[] { new BugTrackerState.Extinguished(), }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugTrackerState.Smoking(), }; }
            }
        }

        [Serializable]
        public class Extinguish : StateTransition<BugTracker, MyAppState>
        {
            public Extinguish(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new[] { new BugTrackerState.Extinguished(), }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugTrackerState.Extinguished(), }; }
            }
        }
    }

    [Serializable]
    public class BugTransition
    {
        [Serializable]
        public class Assign : StateTransition<Bug, MyAppState>
        {
            public Assign(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new BugState[] { new BugState.Open(), new BugState.Assigned(), }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugState.Assigned(), }; }
            }
        }

        [Serializable]
        public class Close : StateTransition<Bug, MyAppState>
        {
            public Close(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new BugState[] { new BugState.Resolved() }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugState.Closed(), }; }
            }
        }

        [Serializable]
        public class Defer : StateTransition<Bug, MyAppState>
        {
            public Defer(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new BugState[] { new BugState.Open(), new BugState.Assigned() }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugState.Deferred(), }; }
            }
        }

        [Serializable]
        public class Open : StateTransition<Bug, MyAppState>
        {
            public Open(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new[] { new BugState.Closed(), }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugState.Open(), }; }
            }
        }

        [Serializable]
        public class Resolve : StateTransition<Bug, MyAppState>
        {
            public Resolve(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override MyAppState[] StartStates
            {
                get { return new[] { new BugState.Assigned(), }; }
            }

            public override MyAppState[] EndStates
            {
                get { return new[] { new BugState.Resolved(), }; }
            }
        }
    }

    //define transition functions (if any)
    public class BugHelper
    {
        public static void Assign(MyAppState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = args.AssigneeEmail;
        }

        public static void Defer(MyAppState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = String.Empty;
        }

        public static void Resolve(MyAppState state, dynamic args)
        {
            args.StatefulObject.AssigneeEmail = String.Empty;
        }

        public static void Close(MyAppState state, dynamic args)
        {
            args.StatefulObject.ClosedByName = args.ClosedByName;
        }
    }

    [TestFixture]
    public class BugTrackerExampleTests
    {
        [SetUp]
        public void Setup()
        {
            var transitions = new IStateTransition<MyAppState>[]
                                  {
                                      new BugTransition.Open(),
                                      new BugTransition.Assign(BugHelper.Assign),
                                      new BugTransition.Defer(BugHelper.Defer),
                                      new BugTransition.Resolve(BugHelper.Resolve),
                                      new BugTransition.Close(BugHelper.Close),
                                  };

            var parentTransitions = new IStateTransition<MyAppState>[]
                                  {
                                      new BugTrackerTransition.SetAlight(),
                                      new BugTrackerTransition.Extinguish(),
                                  };

            var bugTrackerStateMachine = new StateMachine<BugTracker, MyAppState>("BugTracker",
                                                                                  parentTransitions,
                                                                                  startState: new BugTrackerState.Extinguished());
            _stateMachine = new StateMachine<BugTracker, MyAppState>("Bug",
                                                                     transitions,
                                                                     startState: new BugState.Open(),
                                                                     parentStateMachine: bugTrackerStateMachine);
        }

        private StateMachine<BugTracker, MyAppState> _stateMachine;

        [Test]
        public void CurrentState_ImmediatelyAfterConstruction_IsSetToInitialState()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.That(bug.StateMachine.ParentStateMachine.CurrentState == new BugTrackerState.Extinguished());
        }

        [Test]
        public void TriggerTransition_StateTransitionIsUndefined_ExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            Assert.Throws<InvalidStateTransitionException<MyAppState>>(() => bug.TriggerTransition<Bug>(bug, new MyUnrelatedState.Extinguished()));
        }

        [Test]
        public void TriggerTransition_StateTransitionIsUndefined_ExceptionThrown2()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            bug.TriggerTransition<Bug>(bug, new BugState.Assigned(),
                                       new { StatefulObject = bug, AssigneeEmail = "example@example.com" })
               .TriggerTransition<Bug>(bug, new BugTrackerState.Smoking(), new { StatefulObject = bug });

            Assert.Throws<InvalidStateTransitionException<MyAppState>>(() => bug.TriggerTransition<Bug>(bug, new MyUnrelatedState.Extinguished()));
        }

        [Test]
        public void Test()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            Assert.That(bug.CurrentState == new BugState.Open());

            //act/assert
            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.That(_stateMachine.ParentStateMachine.CurrentState == new BugTrackerState.Extinguished());

            bug.TriggerTransition<Bug>(bug, new BugTrackerState.Smoking());

            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.That(_stateMachine.ParentStateMachine.CurrentState == new BugTrackerState.Smoking());
            Assert.That(bug.AssigneeEmail == null);
            Assert.That(_stateMachine.ParentStateMachine.CurrentState == new BugTrackerState.Smoking());

            bug.TriggerTransition<Bug>(bug, new BugState.Assigned(), new { StatefulObject = bug, AssigneeEmail = "example@example.com" });
            Assert.That(bug.CurrentState == new BugState.Assigned());
            Assert.That(bug.AssigneeEmail == "example@example.com");
            Assert.That(_stateMachine.ParentStateMachine.CurrentState == new BugTrackerState.Smoking());
        }

        [Test]
        public void TriggerTransition_InvalidTransition_ExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.Throws<InvalidStateTransitionException<MyAppState>>(
                () => bug.TriggerTransition<Bug>(bug, new BugState.Resolved()));
        }

        [Test]
        public void TriggerTransition_TwoSuccessiveValidTransitions_StateSetCorrectly()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            var assigneeEmail = "example@example.com";

            //act/assert
            bug.TriggerTransition<Bug>(bug, new BugState.Assigned(),
                                       new { StatefulObject = bug, AssigneeEmail = assigneeEmail })
               .TriggerTransition<Bug>(bug, new BugTrackerState.Smoking());

            Assert.That(bug.CurrentState == new BugState.Assigned());
            Assert.That(bug.AssigneeEmail == assigneeEmail);
            Assert.That(bug.ParentState == new BugTrackerState.Smoking());
        }

        //[Test]
        //public void TriggerTransition_ExpectedReturnTypeMismatch_ExceptionThrown()
        //{
        //    //arrange
        //    var bug = new Bug("bug1", _stateMachine);

        //    //act/assert
        //    Assert.Throws<RuntimeBinderException>(() => bug.TriggerTransition<BugTracker>(bug, new BugTrackerState.Extinguished()));
        //}

        [Test]
        public void TriggerTransition_ValidTransitions_NoExceptionThrown()
        {
            //arrange
            var bug = new BugTracker(_stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition<BugTracker>(bug, new BugTrackerState.Extinguished()));
        }

        [Test]
        public void TriggerTransition_ValidFollowedBySubsequentInvalidTransition_ExceptionThrown()
        {
            //arrange
            var bug = new BugTracker(_stateMachine);

            //act/assert
            Assert.Throws<InvalidStateTransitionException<MyAppState>>(() => bug.TriggerTransition<BugTracker>(bug, new BugTrackerState.Smoking())
                .TriggerTransition<BugTracker>(bug, new BugTrackerState.Extinguished()));
        }

        [Test]
        public void TriggerTransition_TwoSuccessiveValidTransitions_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition<Bug>(bug, new BugState.Assigned(),
                                                                 new { StatefulObject = bug, AssigneeEmail = "example@example.com" })
                                         .TriggerTransition<Bug>(bug, new BugState.Deferred(), new { StatefulObject = bug }));
        }

        [Test]
        public void TriggerTransition_IdentityTransition_NoExceptionThrown()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.That(bug.CurrentState == new BugState.Open());
            Assert.DoesNotThrow(() => bug.TriggerTransition<Bug>(bug, new BugState.Open()));
            Assert.That(bug.CurrentState == new BugState.Open());
        }

        [Test]
        public void TriggerTransition_UnexpectedDtoSupplied_NoExceptionThrown() //not sure how I could detect this in the framework
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);

            //act/assert
            Assert.DoesNotThrow(() => bug.TriggerTransition<Bug>(bug, new BugState.Open(), new { Blah = "blah", }));
        }

        [Test]
        public void TriggerTransition_ValidTransitionWithArgument_ArgumentSetInTargetObjectCorrectly()
        {
            //arrange
            var bug = new Bug("bug1", _stateMachine);
            const string assigneeEmail = "example@example.com";

            //act/assert
            bug = bug.TriggerTransition<Bug>(bug, new BugState.Assigned(),
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
            Assert.DoesNotThrow(() => bug.TriggerTransition<Bug>(bug, new BugState.Assigned(),
                                                       new { StatefulObject = bug, AssigneeEmail = "example@example.com" }));
        }

        [Test, Ignore("WIP")]
        public void Serialize_ValidStateMachine_NoExceptionThrown()
        {
            //arrange
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };

            JsonConvert.SerializeObject(_stateMachine, Formatting.Indented, settings);

            //act/assert
            //Assert.DoesNotThrow(() => JsonConvert.SerializeObject(_stateMachine, Formatting.Indented, settings));
        }

        ////these fail due to inability to deserialize the generic type constraint BugState (it would appear)
        //[Test, Ignore]
        //public void DeSerialize_ValidStateMachine_NoExceptionThrown()
        //{
        //    //arrange
        //    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        //    var serializedStateMachine = JsonConvert.SerializeObject(_stateMachine, Formatting.Indented, settings);

        //    //act/assert
        //    Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<StateMachine<IMyAppStateful, MyAppState>>(serializedStateMachine));
        //}

        //[Test, Ignore]
        //public void Serialization_RoundTrip_StateMaintainedCorrectly()
        //{
        //    //arrange
        //    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        //    var serializedStateMachine = JsonConvert.SerializeObject(_stateMachine, Formatting.Indented, settings);

        //    //act
        //    var stateMachine = JsonConvert.DeserializeObject<StateMachine<IMyAppStateful, MyAppState>>(serializedStateMachine);

        //    //assert
        //    Assert.That(stateMachine.ChildStateMachines.Count == 0);
        //    Assert.That(stateMachine.CurrentState == new BugState.Open());
        //}

        //[Test, Ignore]
        //public void Serialization_RoundTrip_StateMaintainedCorrectly2()
        //{
        //    //arrange
        //    var bug = new Bug("bug1", _stateMachine);
        //    bug.TransitionTo(new BugState.Assigned(), new { AssigneeEmail = "example@example.com" });

        //    //assert
        //    Assert.That(_stateMachine.CurrentState == new BugState.Assigned());

        //    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        //    var serializedStateMachine = JsonConvert.SerializeObject(_stateMachine, Formatting.Indented, settings);

        //    //act
        //    var stateMachine = JsonConvert.DeserializeObject<StateMachine<IMyAppStateful, MyAppState>>(serializedStateMachine);

        //    //assert
        //    Assert.That(stateMachine.ChildStateMachines.Count == 0);
        //    Assert.That(stateMachine.CurrentState == new BugState.Assigned());
        //}

        //[Test, Ignore]
        //public void Serialization_RoundTrip_TransitionsMayStillBePerformedAfterDeserialization()
        //{
        //    //arrange
        //    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        //    var serializedStateMachine = JsonConvert.SerializeObject(_stateMachine, Formatting.Indented, settings);

        //    //act
        //    var stateMachine = JsonConvert.DeserializeObject<StateMachine<IMyAppStateful, MyAppState>>(serializedStateMachine);
        //    var bug = new Bug("bug1", stateMachine);

        //    //assert
        //    Assert.That(bug.CurrentState ==  new BugState.Open());

        //    //act
        //    bug.TransitionTo(new BugState.Assigned(), new { AssigneeEmail = "example@example.com", });

        //    Assert.That(bug.CurrentState == new BugState.Assigned());
        //}
    }
}
// ReSharper restore InconsistentNaming