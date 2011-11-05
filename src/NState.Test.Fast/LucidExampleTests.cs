// ReSharper disable InconsistentNaming
namespace NState.Test.Fast
{
    using System;
    using NUnit.Framework;

    public abstract class LucidState : State {}

    public abstract class HomePanelState : LucidState
    {
        public class Hidden : HomePanelState {}

        public class Visible : HomePanelState {}
    }

    public abstract class SearchTabState : LucidState
    {
        public class Hidden : SearchTabState {}

        public class Visible : SearchTabState {}
    }

    public abstract class AccountTabState : LucidState
    {
        public class Hidden : AccountTabState {}

        public class Visible : AccountTabState {}
    }

    public class HomePanel : Stateful<HomePanel, LucidState>
    {
        public HomePanel(IStateMachine<HomePanel, LucidState> stateMachine)
            : base(stateMachine) {}

        public HomePanel Hide()
        {
            return TriggerTransition(this, new HomePanelState.Hidden());
        }

        public HomePanel Show()
        {
            return TriggerTransition(this, new HomePanelState.Visible());
        }
    }

    public class SearchTab : Stateful<HomePanel, LucidState>
    {
        public SearchTab(IStateMachine<HomePanel, LucidState> stateMachine)
            : base(stateMachine) {}

        public SearchTab Hide()
        {
            return TriggerTransition(this, new SearchTabState.Hidden(),
                                     new
                                         {
                                             SearchTabSM = StateMachine,
                                             AccountTabSM =
                                         StateMachine.ParentStateMachine.ChildStateMachines["AccountTab"],
                                         });

            //return this;
        }

        public SearchTab Show()
        {
            return TriggerTransition(this, new SearchTabState.Visible(),
                                     new
                                         {
                                             SearchTabSM = StateMachine,
                                             AccountTabSM =
                                         StateMachine.ParentStateMachine.ChildStateMachines["AccountTab"],
                                         });

            //return this;
        }
    }

    public class AccountTab : Stateful<HomePanel, LucidState>
    {
        public AccountTab(IStateMachine<HomePanel, LucidState> stateMachine)
            : base(stateMachine) {}

        public AccountTab Hide()
        {
            TriggerTransition(this, new AccountTabState.Hidden(),
                              new
                                  {
                                      SearchTabSM = StateMachine.ParentStateMachine.ChildStateMachines["SearchTab"],
                                      AccountTabSM = StateMachine,
                                  });

            return this;
        }

        public AccountTab Show()
        {
            TriggerTransition(this, new AccountTabState.Visible(),
                              new
                                  {
                                      SearchTabSM = StateMachine.ParentStateMachine.ChildStateMachines["SearchTab"],
                                      AccountTabSM = StateMachine,
                                  });

            return this;
        }
    }

    public class HomePanelTransition
    {
        [Serializable]
        public class Hide : StateTransition<HomePanel, LucidState>
        {
            public Hide(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new HomePanelState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new HomePanelState.Hidden(),}; }
            }
        }

        [Serializable]
        public class Show : StateTransition<HomePanel, LucidState>
        {
            public Show(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new HomePanelState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new HomePanelState.Visible(),}; }
            }
        }
    }

    public class SearchTabTransition
    {
        [Serializable]
        public class Hide : StateTransition<SearchTab, LucidState>
        {
            public Hide(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new SearchTabState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new SearchTabState.Hidden(),}; }
            }
        }

        [Serializable]
        public class Show : StateTransition<SearchTab, LucidState>
        {
            public Show(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new SearchTabState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new SearchTabState.Visible(),}; }
            }
        }
    }

    public class AccountTabTransition
    {
        [Serializable]
        public class Hide : StateTransition<AccountTab, LucidState>
        {
            public Hide(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new AccountTabState.Visible(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new AccountTabState.Hidden(),}; }
            }
        }

        [Serializable]
        public class Show : StateTransition<AccountTab, LucidState>
        {
            public Show(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) {}

            public override LucidState[] StartStates
            {
                get { return new[] {new AccountTabState.Hidden(),}; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] {new AccountTabState.Visible(),}; }
            }
        }
    }

    public class SearchTabHelper
    {
        public static void Hide(LucidState state, dynamic args)
        {
            args.AccountTabSM.CurrentState = new AccountTabState.Visible();
        }

        public static void Show(LucidState state, dynamic args)
        {
            args.AccountTabSM.CurrentState = new AccountTabState.Hidden();
        }
    }

    public class AccountTabHelper
    {
        public static void Hide(LucidState state, dynamic args)
        {
            args.SearchTabSM.CurrentState = new SearchTabState.Visible();
        }

        public static void Show(LucidState state, dynamic args)
        {
            args.SearchTabSM.CurrentState = new SearchTabState.Hidden();
        }
    }

    [TestFixture]
    public class SearchTabTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            var homePanelTransitions = new IStateTransition<LucidState>[]
                                           {
                                               new HomePanelTransition.Hide(),
                                               new HomePanelTransition.Show(),
                                           };

            var searchTabTransitions = new IStateTransition<LucidState>[]
                                           {
                                               new SearchTabTransition.Hide(SearchTabHelper.Hide),
                                               new SearchTabTransition.Show(SearchTabHelper.Show),
                                           };

            var accountTabTransitions = new IStateTransition<LucidState>[]
                                            {
                                                new AccountTabTransition.Hide(AccountTabHelper.Hide),
                                                new AccountTabTransition.Show(AccountTabHelper.Show),
                                            };

            _homePanelStateMachine = new StateMachine<HomePanel, LucidState>("HomePanel",
                                                                             homePanelTransitions,
                                                                             startState: new HomePanelState.Visible());

            _searchTabStateMachine = new StateMachine<HomePanel, LucidState>("SearchTab",
                                                                             searchTabTransitions,
                                                                             startState: new SearchTabState.Visible(),
                                                                             parentStateMachine: _homePanelStateMachine);

            //todo: put in bi-directional link when setting parent state machine
            //one way up, multi-way down (e.g. HomePanel might have multiple children)
            _accountTabStateMachine = new StateMachine<HomePanel, LucidState>("AccountTab",
                                                                              accountTabTransitions,
                                                                              startState: new AccountTabState.Visible(),
                                                                              parentStateMachine: _homePanelStateMachine);
        }

        #endregion

        private StateMachine<HomePanel, LucidState> _homePanelStateMachine;
        private StateMachine<HomePanel, LucidState> _searchTabStateMachine;
        private StateMachine<HomePanel, LucidState> _accountTabStateMachine;

        [Test]
        public void AccountTab_ShowHideShowTest()
        {
            //arrange
            var h = new AccountTab(_accountTabStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new AccountTabState.Visible());
        }

        [Test]
        public void AccountTab_ShowHideTest()
        {
            //arrange
            var h = new AccountTab(_accountTabStateMachine);
            h.Hide();

            //act/assert
            Assert.That(h.CurrentState == new AccountTabState.Hidden());
        }

        [Test]
        public void AccountTab_ShowShowTest()
        {
            //arrange
            var h = new AccountTab(_accountTabStateMachine);
            h.Show();

            //act/assert
            Assert.That(h.CurrentState == new AccountTabState.Visible());
        }

        [Test]
        public void InitialState()
        {
            //arrange
            var s = new SearchTab(_searchTabStateMachine);
            var a = new AccountTab(_accountTabStateMachine);

            //act/assert
            Assert.That(s.CurrentState == new SearchTabState.Visible());
            Assert.That(a.CurrentState == new AccountTabState.Visible());
        }

        [Test]
        public void SearchTab_ShowHideShowTest()
        {
            //arrange
            var h = new SearchTab(_searchTabStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new SearchTabState.Visible());
        }

        [Test]
        public void SearchTab_ShowHideTest()
        {
            //arrange
            var h = new SearchTab(_searchTabStateMachine);
            h.Hide();

            //act/assert
            Assert.That(h.CurrentState == new SearchTabState.Hidden());
        }

        [Test]
        public void SearchTab_ShowShowTest()
        {
            //arrange
            var h = new SearchTab(_searchTabStateMachine);
            h.Show();

            //act/assert
            Assert.That(h.CurrentState == new SearchTabState.Visible());
        }

        [Test]
        public void When_SearchIsHidden_AccountIsShown()
        {
            //arrange
            var s = new SearchTab(_searchTabStateMachine);
            var a = new AccountTab(_accountTabStateMachine);

            //assert
            Assert.That(s.CurrentState == new SearchTabState.Visible());
            Assert.That(a.CurrentState == new AccountTabState.Visible());

            //act
            s.Hide();

            //assert
            Assert.That(s.CurrentState == new SearchTabState.Hidden());
            Assert.That(a.CurrentState == new AccountTabState.Visible());

            //act
            s.Show();

            //assert
            Assert.That(s.CurrentState == new SearchTabState.Visible());
            Assert.That(a.CurrentState == new AccountTabState.Hidden());

            //act
            s.Hide();

            //assert
            Assert.That(s.CurrentState == new SearchTabState.Hidden());
            Assert.That(a.CurrentState == new AccountTabState.Visible());

            //act
            a.Hide();

            //assert
            Assert.That(s.CurrentState == new SearchTabState.Visible());
            Assert.That(a.CurrentState == new AccountTabState.Hidden());
        }
    }

    [TestFixture]
    public class LucidHomePanelTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            var homePanelTransitions = new IStateTransition<LucidState>[]
                                           {
                                               new HomePanelTransition.Hide(),
                                               new HomePanelTransition.Show(),
                                           };

            _homePanelStateMachine = new StateMachine<HomePanel, LucidState>("HomePanel",
                                                                             homePanelTransitions,
                                                                             startState: new HomePanelState.Visible());
        }

        #endregion

        private StateMachine<HomePanel, LucidState> _homePanelStateMachine;

        [Test]
        public void InitialState()
        {
            //arrange
            var h = new HomePanel(_homePanelStateMachine);

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }

        [Test]
        public void ShowHideShowTest()
        {
            //arrange
            var h = new HomePanel(_homePanelStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }

        [Test]
        public void ShowHideTest()
        {
            //arrange
            var h = new HomePanel(_homePanelStateMachine);
            h.Hide();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Hidden());
        }

        [Test]
        public void ShowShowTest()
        {
            //arrange
            var h = new HomePanel(_homePanelStateMachine);
            h.Show();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }
    }
}

// ReSharper restore InconsistentNaming