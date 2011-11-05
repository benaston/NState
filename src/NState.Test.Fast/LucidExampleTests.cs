// ReSharper disable InconsistentNaming
namespace NState.Test.Fast
{
    using System;
    using NUnit.Framework;

    public abstract class LucidState : State {}

    public abstract class UIRootState : LucidState
    {
        public class Enabled : UIRootState { }
    }

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

    public abstract class SearchPanelState : LucidState
    {
        public class Hidden : SearchPanelState { }

        public class Visible : SearchPanelState { }
    }

    public abstract class WorkingPanelState : LucidState
    {
        public class SearchMode : WorkingPanelState { }

        public class AccountMode : WorkingPanelState { }
    }

    public abstract class DetailsPanelsState : LucidState
    {
        public class SearchMode : DetailsPanelsState { }

        public class AccountMode : DetailsPanelsState { }
    }

    public class UIRoot : Stateful<UIRoot, LucidState>
    {
        public UIRoot(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) {}

        public UIRoot Hide()
        {
            return TriggerTransition(this, new HomePanelState.Hidden());
        }

        public UIRoot Show()
        {
            return TriggerTransition(this, new HomePanelState.Visible());
        }
    }

    public class HomePanel : Stateful<UIRoot, LucidState>
    {
        public HomePanel(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public HomePanel Hide()
        {
            return TriggerTransition(this, new HomePanelState.Hidden());
        }

        public HomePanel Show()
        {
            return TriggerTransition(this, new HomePanelState.Visible());
        }
    }

    public class SearchTab : Stateful<UIRoot, LucidState>
    {
        public SearchTab(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) {}

        public SearchTab Hide()
        {
            return TriggerTransition(this, new SearchTabState.Hidden(),
                                     new
                                         {
                                             SearchTabSM = StateMachine,
                                             AccountTabSM =
                                         StateMachine.Parent.Children["AccountTab"],
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
                                         StateMachine.Parent.Children["AccountTab"],
                                         });

            //return this;
        }
    }

    public class AccountTab : Stateful<UIRoot, LucidState>
    {
        public AccountTab(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) {}

        public AccountTab Hide()
        {
            TriggerTransition(this, new AccountTabState.Hidden(),
                              new
                                  {
                                      SearchTabSM = StateMachine.Parent.Children["SearchTab"],
                                      AccountTabSM = StateMachine,
                                  });

            return this;
        }

        public AccountTab Show()
        {
            TriggerTransition(this, new AccountTabState.Visible(),
                              new
                                  {
                                      SearchTabSM = StateMachine.Parent.Children["SearchTab"],
                                      AccountTabSM = StateMachine,
                                  });

            return this;
        }
    }

    public class SearchPanel : Stateful<UIRoot, LucidState>
    {
        public SearchPanel(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public SearchPanel Hide()
        {
            TriggerTransition(this, new SearchPanelState.Hidden(),
                              new
                              {
                                  //       search panel search tab         home panel         root
                                  StateMachine, //todo fix this madness - auto update root by walking the tree
                              });

            return this;
        }

        public SearchPanel Show()
        {
            TriggerTransition(this, new SearchPanelState.Visible(),
                              new
                              {
                                  StateMachine,
                              });

            return this;
        }
    }

    public class WorkingPanel : Stateful<UIRoot, LucidState>
    {
        public WorkingPanel(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public WorkingPanel SelectSearchMode()
        {
            TriggerTransition(this, new WorkingPanelState.SearchMode(),
                              new
                              {
                                  StateMachine, //todo fix this madness - auto update root by walking the tree
                              });

            return this;
        }

        public WorkingPanel SelectAccountMode()
        {
            TriggerTransition(this, new WorkingPanelState.AccountMode(),
                              new
                              {
                                  StateMachine,
                              });

            return this;
        }
    }

    public class DetailsPanels : Stateful<UIRoot, LucidState>
    {
        public DetailsPanels(IStateMachine<UIRoot, LucidState> stateMachine)
            : base(stateMachine) { }

        public DetailsPanels SelectSearchMode()
        {
            TriggerTransition(this, new DetailsPanelsState.SearchMode(),
                              new
                              {
                                  StateMachine,
                              });

            return this;
        }

        public DetailsPanels SelectAccountMode()
        {
            TriggerTransition(this, new DetailsPanelsState.AccountMode(),
                              new
                              {
                                  StateMachine,
                              });

            return this;
        }
    }

    public class HomePanelTransition
    {
        [Serializable]
        public class Hide : StateTransition<UIRoot, LucidState>
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
        public class Show : StateTransition<UIRoot, LucidState>
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

    public class SearchPanelTransition
    {
        [Serializable]
        public class Hide : StateTransition<SearchPanel, LucidState>
        {
            public Hide(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new SearchPanelState.Visible(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new SearchPanelState.Hidden(), }; }
            }
        }

        [Serializable]
        public class Show : StateTransition<SearchPanel, LucidState>
        {
            public Show(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new SearchPanelState.Hidden(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new SearchPanelState.Visible(), }; }
            }
        }
    }

    public class WorkingPanelTransition
    {
        [Serializable]
        public class SelectSearchMode : StateTransition<WorkingPanel, LucidState>
        {
            public SelectSearchMode(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new WorkingPanelState.AccountMode(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new WorkingPanelState.SearchMode(), }; }
            }
        }

        [Serializable]
        public class SelectAccountMode : StateTransition<WorkingPanel, LucidState>
        {
            public SelectAccountMode(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new WorkingPanelState.SearchMode(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new WorkingPanelState.AccountMode(), }; }
            }
        }
    }

    public class DetailsPanelsTransition
    {
        [Serializable]
        public class SelectSearchMode : StateTransition<DetailsPanels, LucidState>
        {
            public SelectSearchMode(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new DetailsPanelsState.AccountMode(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new DetailsPanelsState.SearchMode(), }; }
            }
        }

        [Serializable]
        public class SelectAccountMode : StateTransition<DetailsPanels, LucidState>
        {
            public SelectAccountMode(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new DetailsPanelsState.SearchMode(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new DetailsPanelsState.AccountMode(), }; }
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

    public class SearchPanelHelper
    {
        public static void Show(LucidState state, dynamic args)
        {
            //when showing the search panel, ensure the working panel is in the home position?
            //       search panel search tab         home panel         root
            args.StateMachine.Parent.Parent.Parent.Children["WorkingPanel"].TriggerTransition(new WorkingPanelState.SearchMode());
            args.StateMachine.Parent.Parent.Parent.Children["DetailsPanels"].TriggerTransition(new DetailsPanelsState.SearchMode());
        }
    }

    public class WorkingPanelHelper
    {
        public static void SelectSearchMode(LucidState state, dynamic args)
        {
            //reset horiz position
        }

        public static void SelectAccountMode(LucidState state, dynamic args)
        {
            //reset horiz position
        }
    }

    /// <summary>
    /// TODO: make configuration terser.
    /// TODO: supply context/root node by default to transitions (avoiding use of dto in many cases?).
    /// TODO: default all to all transitions? to minimise code
    /// </summary>
    [TestFixture]
    public class SearchTabTests
    {
        private StateMachine<UIRoot, LucidState> _uiRoot;
        private StateMachine<UIRoot, LucidState> _homePanelStateMachine;
        private StateMachine<UIRoot, LucidState> _searchTabStateMachine;
        private StateMachine<UIRoot, LucidState> _accountTabStateMachine;
        private StateMachine<UIRoot, LucidState> _searchPanelStateMachine;
        private StateMachine<UIRoot, LucidState> _workingPanelStateMachine;
        private StateMachine<UIRoot, LucidState> _detailsPanelsStateMachine;

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

            var searchPanelTransitions = new IStateTransition<LucidState>[]
                                            {
                                                new SearchPanelTransition.Hide(),
                                                new SearchPanelTransition.Show(SearchPanelHelper.Show),
                                            };

            var workingPanelTransitions =   new IStateTransition<LucidState>[]
                                            {
                                                new WorkingPanelTransition.SelectSearchMode(WorkingPanelHelper.SelectSearchMode),
                                                new WorkingPanelTransition.SelectAccountMode(WorkingPanelHelper.SelectAccountMode),
                                            };

            var detailsPanelsTransitions = new IStateTransition<LucidState>[]
                                            {
                                                new DetailsPanelsTransition.SelectSearchMode(),
                                                new DetailsPanelsTransition.SelectAccountMode(),
                                            };

            _uiRoot = new StateMachine<UIRoot, LucidState>("Root",
                                                        new IStateTransition<LucidState>[0],
                                                        startState: new UIRootState.Enabled());

            _homePanelStateMachine = new StateMachine<UIRoot, LucidState>("HomePanel",
                                                                      homePanelTransitions,
                                                                      startState: new HomePanelState.Visible(),
                                                                      parentStateMachine: _uiRoot);

            _searchTabStateMachine = new StateMachine<UIRoot, LucidState>("SearchTab",
                                                                             searchTabTransitions,
                                                                             startState: new SearchTabState.Visible(),
                                                                             parentStateMachine: _homePanelStateMachine);

            _accountTabStateMachine = new StateMachine<UIRoot, LucidState>("AccountTab",
                                                                              accountTabTransitions,
                                                                              startState: new AccountTabState.Visible(),
                                                                              parentStateMachine: _homePanelStateMachine);

            _searchPanelStateMachine = new StateMachine<UIRoot, LucidState>("SearchPanel",
                                                                              searchPanelTransitions,
                                                                              startState: new SearchPanelState.Visible(),
                                                                              parentStateMachine: _accountTabStateMachine);

            _workingPanelStateMachine = new StateMachine<UIRoot, LucidState>("WorkingPanel",
                                                                         workingPanelTransitions,
                                                                         startState: new WorkingPanelState.SearchMode(),
                                                                         parentStateMachine: _uiRoot);

            _detailsPanelsStateMachine = new StateMachine<UIRoot, LucidState>("DetailsPanels",
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
        public void SearchPanel_ShowHideShowTest()
        {
            //arrange
            var h = new SearchPanel(_searchPanelStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new SearchPanelState.Visible());
        }

        [Test]
        public void SearchPanel_ShowHideTest()
        {
            //arrange
            var h = new SearchPanel(_searchPanelStateMachine);
            h.Hide();

            //act/assert
            Assert.That(h.CurrentState == new SearchPanelState.Hidden());
        }

        [Test]
        public void SearchPanel_ShowShowTest()
        {
            //arrange
            var h = new SearchPanel(_searchPanelStateMachine);
            h.Show();

            //act/assert
            Assert.That(h.CurrentState == new SearchPanelState.Visible());
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

            _homePanelStateMachine = new StateMachine<UIRoot, LucidState>("HomePanel",
                                                                             homePanelTransitions,
                                                                             startState: new HomePanelState.Visible());
        }

        #endregion

        private StateMachine<UIRoot, LucidState> _homePanelStateMachine;

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