// ReSharper disable InconsistentNaming
namespace NState.Test.Fast.UserInterfaceExample
{
    using NUnit.Framework;

    /// <summary>
    /// TODO: split out these tests.
    /// TODO: make configuration terser.
    /// TODO: supply context/root node by default to transitions (avoiding use of dto in many cases?).
    /// TODO: default all to all transitions? to minimise code
    /// </summary>
    [TestFixture]
    public class SearchTabTests
    {
        private StateMachine<LucidState> _uiRoot;
        private StateMachine<LucidState> _homePanelStateMachine;
        private StateMachine<LucidState> _searchTabStateMachine;
        private StateMachine<LucidState> _accountTabStateMachine;
        private StateMachine<LucidState> _searchPanelStateMachine;
        private StateMachine<LucidState> _workingPanelStateMachine;
        private StateMachine<LucidState> _detailsPanelsStateMachine;

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
                                               new SearchTabTransition.Hide(SearchTabTransitionFunction.Hide),
                                               new SearchTabTransition.Show(SearchTabTransitionFunction.Show),
                                           };

            var accountTabTransitions = new IStateTransition<LucidState>[]
                                            {
                                                new AccountTabTransition.Hide(AccountTabTransitionFunction.Hide),
                                                new AccountTabTransition.Show(AccountTabTransitionFunction.Show),
                                            };

            var searchPanelTransitions = new IStateTransition<LucidState>[]
                                             {
                                                 new SearchAreaTransition.Hide(),
                                                 new SearchAreaTransition.Show(SearchAreaTransitionFunction.Show),
                                             };

            var workingPanelTransitions =   new IStateTransition<LucidState>[]
                                                {
                                                    new WorkingPanelTransition.SelectSearchMode(WorkingPanelTransitionFunction.SelectSearchMode),
                                                    new WorkingPanelTransition.SelectAccountMode(WorkingPanelTransitionFunction.SelectAccountMode),
                                                };

            var detailsPanelsTransitions = new IStateTransition<LucidState>[]
                                               {
                                                   new DetailsPanelsTransition.SelectSearchMode(),
                                                   new DetailsPanelsTransition.SelectAccountMode(),
                                               };

            _uiRoot = new StateMachine<LucidState>("Root",
                                                           new IStateTransition<LucidState>[0],
                                                           startState: new UIRootState.Enabled());

            _homePanelStateMachine = new StateMachine<LucidState>("HomePanel",
                                                                          homePanelTransitions,
                                                                          startState: new HomePanelState.Visible(),
                                                                          parentStateMachine: _uiRoot);

            _searchTabStateMachine = new StateMachine<LucidState>("SearchTab",
                                                                          searchTabTransitions,
                                                                          startState: new SearchTabState.Visible(),
                                                                          parentStateMachine: _homePanelStateMachine);

            _accountTabStateMachine = new StateMachine<LucidState>("AccountTab",
                                                                           accountTabTransitions,
                                                                           startState: new AccountTabState.Visible(),
                                                                           parentStateMachine: _homePanelStateMachine);

            _searchPanelStateMachine = new StateMachine<LucidState>("SearchArea",
                                                                            searchPanelTransitions,
                                                                            startState: new SearchAreaState.Visible(),
                                                                            parentStateMachine: _accountTabStateMachine);

            _workingPanelStateMachine = new StateMachine<LucidState>("WorkingPanel",
                                                                             workingPanelTransitions,
                                                                             startState: new WorkingPanelState.SearchMode(),
                                                                             parentStateMachine: _uiRoot);

            _detailsPanelsStateMachine = new StateMachine<LucidState>("DetailsPanels",
                                                                              detailsPanelsTransitions,
                                                                              startState: new DetailsPanelsState.SearchMode(),
                                                                              parentStateMachine: _uiRoot);
        }

        [Test]
        public void InitialState()
        {
            //arrange
            var s = new SearchTab(_searchTabStateMachine);
            var a = new AccountTab(_accountTabStateMachine);
            var w = new AccountTab(_workingPanelStateMachine);

            //act/assert
            Assert.That(s.CurrentState == new SearchTabState.Visible());
            Assert.That(a.CurrentState == new AccountTabState.Visible());
            Assert.That(w.CurrentState == new WorkingPanelState.SearchMode());
        }

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

        [Test]
        public void SearchArea_ShowHideShowTest()
        {
            //arrange
            var h = new SearchArea(_searchPanelStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new SearchAreaState.Visible());
        }

        [Test]
        public void SearchArea_ShowHideTest()
        {
            //arrange
            var h = new SearchArea(_searchPanelStateMachine);
            h.Hide();

            //act/assert
            Assert.That(h.CurrentState == new SearchAreaState.Hidden());
        }

        [Test]
        public void SearchArea_ShowShowTest()
        {
            //arrange
            var h = new SearchArea(_searchPanelStateMachine);
            h.Show();

            //act/assert
            Assert.That(h.CurrentState == new SearchAreaState.Visible());
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

            _homePanelStateMachine = new StateMachine<LucidState>("HomePanel",
                                                                             homePanelTransitions,
                                                                             startState: new HomePanelState.Visible());
        }

        #endregion

        private StateMachine<LucidState> _homePanelStateMachine;

        [Test]
        public void InitialState()
        {
            //arrange
            var h = new UIRoot(_homePanelStateMachine);

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }

        [Test]
        public void ShowHideShowTest()
        {
            //arrange
            var h = new UIRoot(_homePanelStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }

        [Test]
        public void ShowHideTest()
        {
            //arrange
            var h = new UIRoot(_homePanelStateMachine);
            h.Hide();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Hidden());
        }

        [Test]
        public void ShowShowTest()
        {
            //arrange
            var h = new UIRoot(_homePanelStateMachine);
            h.Show();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }
    }
}

// ReSharper restore InconsistentNaming