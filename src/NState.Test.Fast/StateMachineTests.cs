// ReSharper disable InconsistentNaming
namespace NState.Test.Fast
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using NUnit.Framework;

    //write DSL for construction of the state machine
    //todo: fix generic specification for child state machines
    //TODO: BA; add serialization of the state machine 
    //for persistence and subsequent retrieval.
    [TestFixture]
    public class StateMachineTests
    {
        [Test]
        public void PerformTransition_WhenHappyPathFollowed_StateIsChanged()
        {
            var accountTabStateMachine = new StateMachine
                <AccountTab, AccountTabState>(
                new IStateTransition<AccountTab, AccountTabState>[]
                    {
                        new AccountTabTransitions.Collapse((tab, state, dto) => tab),
                        new AccountTabTransitions.Expand((tab, state, dto) => tab),
                    },
                new AccountTabState.Collapsed());

            var lucidUIStateMachine = new StateMachine<LucidUI, LucidUIState>(
                new IStateTransition<LucidUI, LucidUIState>[]
                    {
                        new LucidUITransitions.Pause((lucidUI, state, dto) => lucidUI),
                        new LucidUITransitions.Resume((lucidUI, state, dto) => lucidUI),
                    },
                new LucidUIState.Paused(),
                new List<IStateMachine> {accountTabStateMachine,});

            //act
            var ui = new LucidUI(lucidUIStateMachine) {AccountTab = new AccountTab(accountTabStateMachine),};
            
            Assert.That(ui.CurrentState == new LucidUIState.Paused(), "lucid start state");

            //act
            ui.TransitionTo(new LucidUIState.Active());
            
            Assert.That(ui.CurrentState == new LucidUIState.Active(), "lucid state post transition");
            Assert.That(ui.AccountTab.CurrentState == new AccountTabState.Collapsed());

            //act
            ui.AccountTab.TransitionTo(new AccountTabState.Expanded());

            Assert.That(ui.AccountTab.CurrentState == new AccountTabState.Expanded(), "accountTab post transition");
        }

        [Test]
        public void PerformTransition_WhenTransitionAffectsOtherPartsOfStateMachine_StateIsChangedInRelevantPlaces()
        {
            var lucidUIStateMachine = new StateMachine<LucidUI, LucidUIState>(
                new IStateTransition<LucidUI, LucidUIState>[]
                    {
                        new LucidUITransitions.Pause((lucidUI, state, dto) => lucidUI),
                        new LucidUITransitions.Resume((lucidUI, state, dto) => lucidUI),
                    },
                new LucidUIState.Paused());

            var savedSearchAStateMachine = new StateMachine
                <SavedSearch, SavedSearchState>(
                new IStateTransition<SavedSearch, SavedSearchState>[]
                    {
                        new SavedSearchTransitions.Expand((ss, state, dto) =>
                                                              {
                                                                  foreach (var i in ss.UIContext
                                                                      .SavedSearches
                                                                      .SkipWhile(s => s.Id == ss.Id).ToList())
                                                                  {
                                                                      i.TransitionTo(
                                                                          new SavedSearchState.Collapsed());
                                                                  }

                                                                  return ss;
                                                              }),
                        new SavedSearchTransitions.Collapse((ss, state, dto) => ss),
                    },
                new SavedSearchState.Collapsed(), childStateMachines: null, parentStateMachines: null);

            var savedSearchBStateMachine = new StateMachine
                <SavedSearch, SavedSearchState>(
                new IStateTransition<SavedSearch, SavedSearchState>[]
                    {
                        new SavedSearchTransitions.Expand((ss, state, dto) =>
                                                              {
                                                                  foreach (var i in ss.UIContext
                                                                      .SavedSearches
                                                                      .SkipWhile(s => s.Id == ss.Id).ToList())
                                                                  {
                                                                      i.TransitionTo(
                                                                          new SavedSearchState.Collapsed());
                                                                  }

                                                                  return ss;
                                                              }),
                        new SavedSearchTransitions.Collapse((ss, state, dto) => ss),
                    },
                new SavedSearchState.Collapsed(), childStateMachines: null, parentStateMachines: null);

            lucidUIStateMachine.ChildStateMachines.Add(savedSearchAStateMachine);
            lucidUIStateMachine.ChildStateMachines.Add(savedSearchBStateMachine);

            var ui = new LucidUI(lucidUIStateMachine);
            ui.SavedSearches = new List<SavedSearch>
                                   {
                                       new SavedSearch(savedSearchAStateMachine, ui) {Id = "a"},
                                       new SavedSearch(savedSearchBStateMachine, ui) {Id = "b"}
                                   };

            Assert.That(ui.CurrentState == new LucidUIState.Paused(), "lucid start state");

            lucidUIStateMachine.TransitionTo(ui, new LucidUIState.Active());
            Assert.That(ui.CurrentState == new LucidUIState.Active(), "lucid post transition state");

            var savedSearchA = ui.SavedSearches.First(s => s.Id == "a");
            var savedSearchB = ui.SavedSearches.First(s => s.Id == "b");

            Assert.That(savedSearchA.CurrentState == new SavedSearchState.Collapsed());
            Assert.That(savedSearchB.CurrentState == new SavedSearchState.Collapsed());

            //act
            savedSearchA.TransitionTo(new SavedSearchState.Expanded());

            Assert.That(savedSearchA.CurrentState == new SavedSearchState.Expanded());
            Assert.That(savedSearchB.CurrentState == new SavedSearchState.Collapsed());

            //act
            savedSearchB.TransitionTo(new SavedSearchState.Expanded());

            Assert.That(savedSearchB.CurrentState == new SavedSearchState.Expanded());
            Assert.That(savedSearchA.CurrentState == new SavedSearchState.Collapsed());
        }

        //http://stackoverflow.com/questions/6404881/custom-conversion-of-specific-objects-in-json-net
        //todo simplify object for serialization or come up with a better way
        //get json.net to persist specific fields and then have custom parser
        [Test, Ignore("WIP")]
        public void Serialize_WhenInvoked_SerializesStateMachineStateToAString()
        {
            var lucidUIStateMachine = new StateMachine<LucidUI, LucidUIState>(
                new IStateTransition<LucidUI, LucidUIState>[]
                    {
                        new LucidUITransitions.Pause((lucidUI, state, dto) => lucidUI),
                        new LucidUITransitions.Resume((lucidUI, state, dto) => lucidUI),
                    },
                new LucidUIState.Paused());

            var savedSearchAStateMachine = new StateMachine
                <SavedSearch, SavedSearchState>(
                new IStateTransition<SavedSearch, SavedSearchState>[]
                    {
                        new SavedSearchTransitions.Expand((ss, state, dto) =>
                                                              {
                                                                  foreach (var i in ss.UIContext
                                                                      .SavedSearches
                                                                      .SkipWhile(s => s.Id == ss.Id).ToList())
                                                                  {
                                                                      i.TransitionTo(
                                                                          new SavedSearchState.Collapsed());
                                                                  }
                                                                  return ss;
                                                              }),
                        new SavedSearchTransitions.Collapse((ss, state, dto) => ss),
                    },
                new SavedSearchState.Collapsed(), childStateMachines: null, parentStateMachines: null);

            var savedSearchBStateMachine = new StateMachine
                <SavedSearch, SavedSearchState>(
                new IStateTransition<SavedSearch, SavedSearchState>[]
                    {
                        new SavedSearchTransitions.Expand((ss, state, dto) =>
                                                              {
                                                                  foreach (var i in ss.UIContext
                                                                      .SavedSearches
                                                                      .SkipWhile(s => s.Id == ss.Id).ToList())
                                                                  {
                                                                      i.TransitionTo(
                                                                          new SavedSearchState.Collapsed());
                                                                  }
                                                                  return ss;
                                                              }),
                        new SavedSearchTransitions.Collapse((ss, state, dto) => ss),
                    },
                new SavedSearchState.Collapsed(), childStateMachines: null, parentStateMachines: null);

            //lucidUIStateMachine.ChildStateMachines.Add(savedSearchAStateMachine);
            //lucidUIStateMachine.ChildStateMachines.Add(savedSearchBStateMachine);

            var ui = new LucidUI(lucidUIStateMachine);
            ui.SavedSearches = new List<SavedSearch>
                                   {
                                       new SavedSearch(savedSearchAStateMachine, ui) {Id = "a"},
                                       new SavedSearch(savedSearchBStateMachine, ui) {Id = "b"}
                                   };

            Assert.That(ui.CurrentState == new LucidUIState.Paused(), "lucid start state");

            lucidUIStateMachine.TransitionTo(ui, new LucidUIState.Active());
            Assert.That(ui.CurrentState == new LucidUIState.Active(), "lucid post transition state");

            var savedSearchA = ui.SavedSearches.First(s => s.Id == "a");
            var savedSearchB = ui.SavedSearches.First(s => s.Id == "b");
            
            Assert.That(savedSearchA.CurrentState == new SavedSearchState.Collapsed());
            Assert.That(savedSearchB.CurrentState == new SavedSearchState.Collapsed());
            
            //act
            savedSearchA.TransitionTo(new SavedSearchState.Expanded());

            //wip...
            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
            var serializedStateMachine = JsonConvert.SerializeObject(lucidUIStateMachine, Formatting.Indented, settings);

            var ooo =
                JsonConvert.DeserializeObject
                    <StateMachine<LucidUI, LucidUIState>>(
                        serializedStateMachine);
            //var newUI = new LucidUI();
            //Assert.That(savedSearchA.CurrentState == new SavedSearchState.Collapsed());
            //Assert.That(savedSearchB.CurrentState == new SavedSearchState.Collapsed());
        }
    }
}
// ReSharper restore InconsistentNaming