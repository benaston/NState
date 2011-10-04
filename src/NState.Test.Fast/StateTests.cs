// ReSharper disable InconsistentNaming
namespace NState.Test.Fast
{
    using System.Collections.Generic;
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
                        //Sam syntax idea
                        new AccountTabTransitions.Collapse(), 
                        new AccountTabTransitions.Expand(),
                    }, 
                new AccountTabState.Collapsed(), null);
            var lucidUIStateMachine = new StateMachine<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>(
                new IStateTransition<LucidUI, LucidUIState, LucidUI, LucidUIState, StateMachineType>[]
                    {
                        new LucidUITransitions.Pause(), 
                        new LucidUITransitions.Resume(),
                    }, 
                new LucidUIState.Paused(),
                new Dictionary<StateMachineType, IStateMachine> { {StateMachineType.AccountTab, accountTabStateMachine}, });

            var ui = new LucidUI();
            Assert.That(ui.GetStateMachine(lucidUIStateMachine).Equals(new LucidUIState.Paused()));
            ui = lucidUIStateMachine.PerformTransition(ui, new LucidUIState.Active());
            Assert.That(ui.GetStateMachine(lucidUIStateMachine) == new LucidUIState.Active());
            var accountTab = new AccountTab();
            Assert.That(accountTab.GetStateMachine(lucidUIStateMachine).CurrentState == new AccountTabState.Collapsed());
            accountTab = accountTab.PerformTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>(new AccountTabState.Expanded(), lucidUIStateMachine);
            Assert.That(accountTab.GetStateMachine(lucidUIStateMachine).CurrentState == new AccountTabState.Expanded());
        }

        //tests for event registration/invocation and transition performance, per old tests
    }

    public static class StateExtensions
    {
        public static TStatefulDomainObject PerformTransition<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnumeration>
            (this TStatefulDomainObject statefulDomainObject, TState targetState, IStateMachine<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnumeration> stateMachine)
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