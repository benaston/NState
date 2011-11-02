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
        //class StateMachine<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum> :
        //IStateMachine<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        //where TStatefulDomainObject : IStateful<TStatefulDomainObject, TState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        //where TState : State
        //where TBaseDomainObject : IStateful<TBaseDomainObject, TBaseState, TBaseDomainObject, TBaseState, TStateMachineTypeEnum>
        //where TBaseState : State
        //where TStateMachineTypeEnum : struct

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
            Assert.That(ui.GetStateMachineFromRootComposite(lucidUIStateMachine).CurrentState == new LucidUIState.Paused(), "lucid start state");
            ui = lucidUIStateMachine.PerformTransition(ui, new LucidUIState.Active());
            Assert.That(ui.GetStateMachineFromRootComposite(lucidUIStateMachine).CurrentState == new LucidUIState.Active(), "lucid state post transition");
            var accountTab = new AccountTab();
            Assert.That(accountTab.GetStateMachineFromRootComposite(lucidUIStateMachine).CurrentState == new AccountTabState.Collapsed());
            accountTab = accountTab.PerformTransition<AccountTab, AccountTabState, LucidUI, LucidUIState, StateMachineType>
                (new AccountTabState.Expanded(), lucidUIStateMachine);
            Assert.That(accountTab.GetStateMachineFromRootComposite(lucidUIStateMachine).CurrentState == new AccountTabState.Expanded(), "accountTab post transition");
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
            return statefulDomainObject.GetStateMachineFromRootComposite(stateMachine).PerformTransition(statefulDomainObject, targetState);
        }
    }
}

// ReSharper restore InconsistentNaming