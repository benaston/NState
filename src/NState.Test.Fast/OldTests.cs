
//[TestFixture]
//public class StateMachineTransitionTests
//{
//    private TestItem _testItem;
//    private TestStateMachine _stateMachine;

//    [SetUp]
//    public void Setup()
//    {
//        //arrange
//        _testItem = new TestItem();
//        _stateMachine =
//            new TestStateMachine(() => new IStateTransition<ITestItem, TestItemState>[] { new OneToTwoTransition(), new TwoToFourTransition(), });

//        //assert start state
//        Assert.That(_testItem.CurrentState == TestItemState.State1);
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState1To1_DoesNotThrowAnException()
//    {
//        //arrange in setup

//        //act / assert
//        Assert.DoesNotThrow(() => _stateMachine.PerformTransition(_testItem, TestItemState.State1), "State 1 to 1");
//        Assert.That(_testItem.CurrentState == TestItemState.State1);
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState1To2_OK()
//    {
//        //arrange in setup

//        //act / assert
//        Assert.DoesNotThrow(() => _stateMachine.PerformTransition(_testItem, TestItemState.State2), "State 1 to 2");
//        Assert.That(_testItem.CurrentState == TestItemState.State2);
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState1To3_ThrowsException()
//    {
//        //arrange in setup

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State3), "State 1 to 3");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState1To4_ThrowsException()
//    {
//        //arrange in setup

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State4), "State 1 to 4");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState2To1_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State2;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State1), "State 2 to 1");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState2To2_DoesNotThrowAnException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State2;

//        //act / assert
//        Assert.DoesNotThrow(() => _stateMachine.PerformTransition(_testItem, TestItemState.State2), "State 2 to 2");
//        Assert.That(_testItem.CurrentState == TestItemState.State2);
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState2To3_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State2;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State3), "State 2 to 3");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState2To4_DoesNotThrowException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State2;

//        //act / assert
//        Assert.DoesNotThrow(() => _stateMachine.PerformTransition(_testItem, TestItemState.State4), "State 2 to 4");
//        Assert.That(_testItem.CurrentState == TestItemState.State4);
//    }

//    //base state 3
//    [Test]
//    public void PerformTransition_TransitionsAttemptedState3To1_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State3;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State1), "State 3 to 1");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState3To2_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State3;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State2), "State 3 to 2");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState3To3_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State3;

//        //act / assert
//        Assert.DoesNotThrow(() => _stateMachine.PerformTransition(_testItem, TestItemState.State3), "State 3 to 3");
//        Assert.That(_testItem.CurrentState == TestItemState.State3);
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState3To4_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State3;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State4), "State 3 to 4");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState4To1_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State4;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State4), "State 4 to 1");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState4To2_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State4;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State4), "State 4 to 2");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState4To3_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State4;

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(() => _stateMachine.PerformTransition(_testItem, TestItemState.State3), "State 4 to 3");
//    }

//    [Test]
//    public void PerformTransition_TransitionsAttemptedState4To4_ThrowsException()
//    {
//        //arrange in setup
//        _testItem.CurrentState = TestItemState.State4;

//        //act / assert
//        Assert.DoesNotThrow(() => _stateMachine.PerformTransition(_testItem, TestItemState.State4), "State 4 to 4");
//        Assert.That(_testItem.CurrentState == TestItemState.State4);
//    }
//}

//[TestFixture]
//public class StateMachineTests
//{
//    [Test]
//    public void PerformTransition_InvalidTransitionAttempted_ExceptionIsThrown()
//    {
//        //arrange
//        var t = new TestItem();
//        var s =
//            new TestStateMachine(() => new IStateTransition<ITestItem, TestItemState>[] {new OneToTwoTransition(),});

//        //assert start state
//        Assert.That(t.CurrentState == TestItemState.State1);

//        //act / assert
//        Assert.Throws<InvalidStateTransitionException<TestItemState>>(
//            () => s.PerformTransition(t, TestItemState.State3));
//    }

//    [Test]
//    public void PerformTransition_ValidTransitionAttempted_ExceptionIsNotThrownAndStateIsUpdatedOK()
//    {
//        //arrange
//        ITestItem t = new TestItem();
//        var s =
//            new TestStateMachine(() => new IStateTransition<ITestItem, TestItemState>[] { new OneToTwoTransition(), });

//        //assert
//        Assert.That(t.CurrentState == TestItemState.State1);

//        //act
//        t = s.PerformTransition(t, TestItemState.State2);

//        //assert
//        Assert.That(t.CurrentState == TestItemState.State2);
//    }

//    [Test]
//    public void PerformTransition_TransitionAttemptedToCurrentState_ExceptionIsNotThrownAndStateRemainsUnchanged()
//    {
//        //arrange
//        ITestItem t = new TestItem();
//        var s =
//            new TestStateMachine(() => new IStateTransition<ITestItem, TestItemState>[] { new OneToTwoTransition(), });

//        //assert
//        Assert.That(t.CurrentState == TestItemState.State1);

//        //act
//        t = s.PerformTransition(t, TestItemState.State1);

//        //assert
//        Assert.That(t.CurrentState == TestItemState.State1);
//    }
//}

//public class TestStateMachine : StateMachine<ITestItem, TestItemState>
//{
//    /// <summary>
//    ///   Transitions to be injected by IOC container.
//    /// </summary>
//    public TestStateMachine(
//        Func<IEnumerable<IStateTransition<ITestItem, TestItemState>>> stateTransitionRetrievalFunc)
//        : base(stateTransitionRetrievalFunc) {}
//}

//public class OneToTwoTransition : IStateTransition<ITestItem, TestItemState>
//{
//    public IEnumerable<TestItemState> StartState
//    {
//        get { return new[] {TestItemState.State1}; }
//    }

//    public IEnumerable<TestItemState> EndState
//    {
//        get { return new[] {TestItemState.State2}; }
//    }

//    public Func<ITestItem, TestItemState, ITestItem> Transition
//    {
//        get { return TransitionMethod; }
//    }

//    private static ITestItem TransitionMethod(ITestItem opportunity, TestItemState targetState)
//    {
//        if (opportunity.CurrentState == targetState) return opportunity;

//        opportunity.CurrentState = targetState;

//        return opportunity;
//    }
//}

//public class TwoToFourTransition : IStateTransition<ITestItem, TestItemState>
//{
//    public IEnumerable<TestItemState> StartState
//    {
//        get { return new[] {TestItemState.State2}; }
//    }

//    public IEnumerable<TestItemState> EndState
//    {
//        get { return new[] {TestItemState.State4}; }
//    }

//    public Func<ITestItem, TestItemState, ITestItem> Transition
//    {
//        get { return TransitionMethod; }
//    }

//    private static ITestItem TransitionMethod(ITestItem opportunity, TestItemState targetState)
//    {
//        if (opportunity.CurrentState == targetState) return opportunity;

//        opportunity.CurrentState = targetState;

//        return opportunity;
//    }
//}
