// ReSharper disable InconsistentNaming
namespace NState.Test.Fast.Test
{
    using System;
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using NUnit.Framework;


    public abstract class LucidState : State { }

    //public abstract class MyUnrelatedState : State
    //{
    //    public class Extinguished : BugTrackerState { }

    //    public class Smoking : BugTrackerState { }
    //}

    //public abstract class MyOtherBugTrackerState : LucidState
    //{
    //    public class Extinguished : BugTrackerState { }

    //    public class Smoking : BugTrackerState { }
    //}

    public abstract class HomePanelState : LucidState
    {
        public class Visible : HomePanelState { }

        public class Hidden : HomePanelState { }
    }

    //[Serializable]
    //public abstract class BugState : LucidState
    //{
    //    public class Assigned : BugState {}

    //    public class Closed : BugState {}

    //    public class Deferred : BugState {}

    //    public class Open : BugState {}

    //    public class Resolved : BugState {}
    //}

    public class HomePanel : Stateful<HomePanel, LucidState>
    {
        public HomePanel(IStateMachine<HomePanel, LucidState> stateMachine)
            : base(stateMachine)
        {
        }

        public HomePanel Hide()
        {
            return TriggerTransition<HomePanel>(this, new HomePanelState.Hidden());
        }

        public HomePanel Show()
        {
            return TriggerTransition<HomePanel>(this, new HomePanelState.Visible());
        }
    }

    //public class Bug : Stateful<BugTracker, LucidState>
    //{
    //    public Bug(string title, IStateMachine<BugTracker, LucidState> stateMachine)
    //        : base(stateMachine)
    //    {
    //        Title = title;
    //    }

    //    public string Title { get; set; }

    //    public string AssigneeEmail { get; set; }

    //    public string ClosedByName { get; set; }

    //    public void Assign(string assigneeEmail)
    //    {
    //        TriggerTransition<Bug>(new BugState.Assigned(), new { AssigneeEmail = assigneeEmail });
    //    }

    //    public void Defer()
    //    {
    //        TriggerTransition<Bug>(new BugState.Deferred());
    //    }

    //    public void Resolve()
    //    {
    //        TriggerTransition<Bug>(new BugState.Resolved());
    //    }

    //    public void Close(string closedByName)
    //    {
    //        TriggerTransition<Bug>(new BugState.Closed(), new { ClosedByName = closedByName });
    //    }
    //}

    public class HomePanelTransition
    {
        [Serializable]
        public class Hide : StateTransition<HomePanel, LucidState>
        {
            public Hide(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new HomePanelState.Visible(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new HomePanelState.Hidden(), }; }
            }
        }

        [Serializable]
        public class Show : StateTransition<HomePanel, LucidState>
        {
            public Show(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

            public override LucidState[] StartStates
            {
                get { return new[] { new HomePanelState.Hidden(), }; }
            }

            public override LucidState[] EndStates
            {
                get { return new[] { new HomePanelState.Visible(), }; }
            }
        }
    }

    //[Serializable]
    //public class BugTransition
    //{
    //    [Serializable]
    //    public class Assign : StateTransition<Bug, LucidState>
    //    {
    //        public Assign(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

    //        public override LucidState[] StartStates
    //        {
    //            get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
    //        }

    //        public override LucidState[] EndStates
    //        {
    //            get { return new[] {new BugState.Assigned(),}; }
    //        }
    //    }

    //    [Serializable]
    //    public class Close : StateTransition<Bug, LucidState>
    //    {
    //        public Close(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

    //        public override LucidState[] StartStates
    //        {
    //            get { return new BugState[] { new BugState.Resolved() }; }
    //        }

    //        public override LucidState[] EndStates
    //        {
    //            get { return new[] { new BugState.Closed(), }; }
    //        }
    //    }

    //    [Serializable]
    //    public class Defer : StateTransition<Bug, LucidState>
    //    {
    //        public Defer(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

    //        public override LucidState[] StartStates
    //        {
    //            get { return new BugState[] { new BugState.Open(), new BugState.Assigned() }; }
    //        }

    //        public override LucidState[] EndStates
    //        {
    //            get { return new[] { new BugState.Deferred(), }; }
    //        }
    //    }

    //    [Serializable]
    //    public class Open : StateTransition<Bug, LucidState>
    //    {
    //        public Open(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

    //        public override LucidState[] StartStates
    //        {
    //            get { return new[] {new BugState.Closed(),}; }
    //        }

    //        public override LucidState[] EndStates
    //        {
    //            get { return new[] {new BugState.Open(),}; }
    //        }
    //    }

    //    [Serializable]
    //    public class Resolve : StateTransition<Bug, LucidState>
    //    {
    //        public Resolve(Action<LucidState, dynamic> transitionFunction = null) : base(transitionFunction) { }

    //        public override LucidState[] StartStates
    //        {
    //            get { return new[] { new BugState.Assigned(), }; }
    //        }

    //        public override LucidState[] EndStates
    //        {
    //            get { return new[] { new BugState.Resolved(), }; }
    //        }
    //    }
    //}

    //define transition functions (if any)
    //public class BugHelper
    //{
    //    public static void Assign(LucidState state, dynamic args)
    //    {
    //        args.StatefulObject.AssigneeEmail = args.AssigneeEmail;
    //    }

    //    public static void Defer(LucidState state, dynamic args)
    //    {
    //        args.StatefulObject.AssigneeEmail = String.Empty;
    //    }

    //    public static void Resolve(LucidState state, dynamic args)
    //    {
    //        args.StatefulObject.AssigneeEmail = String.Empty;
    //    }

    //    public static void Close(LucidState state, dynamic args)
    //    {
    //        args.StatefulObject.ClosedByName = args.ClosedByName;
    //    }
    //}

    [TestFixture]
    public class LucidHomePanelTests
    {
        //private StateMachine<BugTracker, LucidState> _stateMachine;
        private StateMachine<HomePanel, LucidState> _homePanelStateMachine;

        [SetUp]
        public void Setup()
        {
            var homePanelTransitions = new IStateTransition<LucidState>[]
                                  {
                                      new HomePanelTransition.Hide(),
                                      new HomePanelTransition.Show(),
                                  };

            //var parentTransitions = new IStateTransition<LucidState>[]
            //                      {
            //                          new BugTrackerTransition.SetAlight(),
            //                          new BugTrackerTransition.Extinguish(),
            //                      };

            _homePanelStateMachine = new StateMachine<HomePanel, LucidState>("HomePanel",
                                                                             homePanelTransitions,
                                                                             startState: new HomePanelState.Visible());
            //_stateMachine = new StateMachine<BugTracker, LucidState>(transitions, 
            //                                                         startState:new BugState.Open(), 
            //                                                         parentStateMachine:bugTrackerStateMachine);
        }



        [Test]
        public void InitialState()
        {
            //arrange
            var h = new HomePanel(_homePanelStateMachine);

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
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
        public void ShowHideShowTest()
        {
            //arrange
            var h = new HomePanel(_homePanelStateMachine).Hide().Show();

            //act/assert
            Assert.That(h.CurrentState == new HomePanelState.Visible());
        }
    }
}
// ReSharper restore InconsistentNaming