// ReSharper disable InconsistentNaming
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
            public override LucidState[] StartStates
            {
                get { return new[] {new SmState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new SmState.Hidden(),}; }
            }
        }

        public class Show : StateTransition<LucidState>
        {
            public override LucidState[] StartStates
            {
                get { return new[] {new SmState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new SmState.Visible(),}; }
            }
        }
    }

    [TestFixture]
    public class SerializationExampleTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _stateMachineRoot = new StateMachine<LucidState>("Root",
                                                             new IStateTransition<LucidState>[0],
                                                             startState: new UIRootState.Enabled());

            _transitions = new IStateTransition<LucidState>[]
                               {
                                   new SmTransition.Hide(),
                                   new SmTransition.Show(),
                               };

            _stateMachine1 = new StateMachine<LucidState>("SM1",
                                                          _transitions,
                                                          startState: new SmState.Visible(),
                                                          parentStateMachine: _stateMachineRoot);
        }

        #endregion

        private StateMachine<LucidState> _stateMachine1;
        private StateMachine<LucidState> _stateMachineRoot;
        private IStateTransition<LucidState>[] _transitions;


        [Test]
        public void DeserializeTest()
        {
            //avoid name clashes when setting parents later in test
            var rootSM2 = new StateMachine<LucidState>("Root",
                                                       new IStateTransition<LucidState>[0],
                                                       startState: new UIRootState.Enabled());

            Assert.That(_stateMachine1.CurrentState == new SmState.Visible());

            _stateMachine1.TriggerTransition(new SmState.Hidden());

            Assert.That(_stateMachine1.CurrentState == new SmState.Hidden());

            //arrange
            var json = _stateMachine1.SerializeToJsonDto();

            var sm2 = new StateMachine<LucidState>("SM1",
                                                   _transitions,
                                                   startState: new SmState.Visible(),
                                                   parentStateMachine: rootSM2);

            Assert.That(sm2.CurrentState == new SmState.Visible());

            sm2.InitializeWithJson(json);

            Assert.That(sm2.CurrentState == new SmState.Hidden());
        }

        [Test]
        public void SerializeTest()
        {
            //arrange
            Assert.DoesNotThrow(() => _stateMachineRoot.SerializeToJsonDto());
        }
    }
}

// ReSharper restore InconsistentNaming