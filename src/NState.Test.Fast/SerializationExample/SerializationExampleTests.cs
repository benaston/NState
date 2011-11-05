// ReSharper disable InconsistentNaming
namespace NState.Test.Fast.SerializationExample
{
    using BugTrackerExample;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class SerializationExampleTests
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