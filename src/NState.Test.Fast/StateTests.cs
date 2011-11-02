// ReSharper disable InconsistentNaming
namespace NState.Test.Fast
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    //write DSL for construction of the state machine
    //todo: fix generic specification for child state machines
    //TODO: BA; add serialization of the state machine for persistence and subsequent retrieval.
    [TestFixture]
    public class Tests
    {
        [Test]
        public void PerformTransition_WhenHappyPathFollowed_StateIsChanged()
        {
            var accountTabStateMachine = new StateMachine<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>(
                new IStateTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>[]
                    {
                        new AccountTabTransitions.Collapse((tab,state) => tab), 
                        new AccountTabTransitions.Expand((tab,state) => tab),
                    }, 
                new AccountTabState.Collapsed(), null);
            var lucidUIStateMachine = new StateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>(
                new IStateTransition<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>[]
                    {
                        new LucidUITransitions.Pause((lucidUI,state) => lucidUI), 
                        new LucidUITransitions.Resume((lucidUI,state) => lucidUI),
                    }, 
                new LucidUIState.Paused(),
                new Dictionary<StateMachineType, IStateMachine> { {StateMachineType.AccountTab, accountTabStateMachine}, });

            var ui = new LucidUI();
            Assert.That(ui.GetStateMachine(lucidUIStateMachine).CurrentState == new LucidUIState.Paused(), "lucid start state");
            ui = lucidUIStateMachine.PerformTransition(ui, new LucidUIState.Active());
            Assert.That(ui.GetStateMachine(lucidUIStateMachine).CurrentState == new LucidUIState.Active(), "lucid state post transition");
            var accountTab = new AccountTab();
            Assert.That(accountTab.GetStateMachine(lucidUIStateMachine).CurrentState == new AccountTabState.Collapsed());
            accountTab = accountTab.PerformTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
                (new AccountTabState.Expanded(), lucidUIStateMachine);
            Assert.That(accountTab.GetStateMachine(lucidUIStateMachine).CurrentState == new AccountTabState.Expanded(), "accountTab post transition");
        }

        /// <summary>
        /// The "domain object model" represents the UI elements. The state machine 
        /// mereley controls the "transitions" that may be performed on these domain
        /// objects.
        /// In order to enable trivial ui state persistence, then we need a single 
        /// state machine per domain stateful object?
        /// </summary>
        [Test]
        public void PerformTransition_WhenTransitionAffectsOtherPartsOfStateMachine_StateIsChangedInRelevantPlaces()
        {
            var lucidUIStateMachine = new StateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>(
                new IStateTransition<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>[]
                    {
                        new LucidUITransitions.Pause((lucidUI,state) => lucidUI), 
                        new LucidUITransitions.Resume((lucidUI,state) => lucidUI),
                    },
                new LucidUIState.Paused());

            var savedSearchStateMachine = new StateMachine<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>(
                new IStateTransition<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>[]
                    {
                        new SavedSearchTransitions.Expand((ss,state) =>
                                                                {
                                                                    ss.UiContext
                                                                        .SavedSearches
                                                                        .SkipWhile(s => s.Id == ss.Id)
                                                                        .Select(
                                                                            i =>
                                                                            i.PerformTransition
                                                                                <SavedSearch, SavedSearchState, LucidUI,
                                                                                LucidUIState, StateMachineType>(
                                                                                    new SavedSearchState.Collapsed(),
                                                                                    lucidUIStateMachine));
                                                                    return ss;
                                                                }), 
                        new SavedSearchTransitions.Collapse((ss,state) => ss),
                    },
                new SavedSearchState.Collapsed(), childStateMachines: null);

            lucidUIStateMachine.ChildStateMachines.Add(StateMachineType.SavedSearch, savedSearchStateMachine);
            var ui = new LucidUI();
            ui.SavedSearches = new List<SavedSearch> { new SavedSearch(ui) { Id = "a" }, new SavedSearch(ui) { Id = "b" } } ;

            Assert.That(ui.GetStateMachine(lucidUIStateMachine).CurrentState == new LucidUIState.Paused(), "lucid start state");

            //todo: each stateful object to get its own state machine and expose perform transition
            ui = lucidUIStateMachine.PerformTransition(ui, new LucidUIState.Active());
            Assert.That(ui.GetStateMachine(lucidUIStateMachine).CurrentState == new LucidUIState.Active(), "lucid post transition state");

            var savedSearchA = ui.SavedSearches
                          .First(s => s.Id == "a");
            Assert.That(savedSearchA.GetStateMachine(lucidUIStateMachine).CurrentState == new SavedSearchState.Collapsed());
            savedSearchA = savedSearchA.PerformTransition<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>
                (new SavedSearchState.Expanded(), lucidUIStateMachine);
            Assert.That(savedSearchA.GetStateMachine(lucidUIStateMachine).CurrentState == new SavedSearchState.Expanded());

            var savedSearchB = ui.SavedSearches
                          .First(s => s.Id == "b");
            Assert.That(savedSearchB.GetStateMachine(lucidUIStateMachine).CurrentState == new SavedSearchState.Collapsed());

            Assert.That(savedSearchA.GetStateMachine(lucidUIStateMachine).CurrentState == new SavedSearchState.Expanded());
            savedSearchB = savedSearchB.PerformTransition<SavedSearch, SavedSearchState, LucidUI, LucidUIState, StateMachineType>
                (new SavedSearchState.Expanded(), lucidUIStateMachine);
            Assert.That(savedSearchB.GetStateMachine(lucidUIStateMachine).CurrentState == new SavedSearchState.Expanded());
            Assert.That(savedSearchA.GetStateMachine(lucidUIStateMachine).CurrentState == new SavedSearchState.Collapsed());
        }

        //tests for event registration/invocation and transition performance, per old tests
    }

    public static class StateExtensions
    {
        public static TStatefulDomainObject PerformTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnumeration>
            (this TStatefulDomainObject statefulDomainObject, 
             TState targetState, 
             IStateMachine<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnumeration> stateMachine)
            where TStatefulDomainObject : IStateful<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnumeration>
            where TState : State
            where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnumeration>
            where TBaseState : State
            where TStateMachineTypeEnumeration : struct
        {
            return statefulDomainObject.GetStateMachine(stateMachine).PerformTransition(statefulDomainObject, targetState);
        }
    }
}

// ReSharper restore InconsistentNaming