NState
=====

Simple state machine for .NET.

Usage:
--------

PLEASE NOTE: this project is unfinished and I realise that the API is currently pretty horrible.

*Create some state transitions*

```C#
	var myTransitions = new IStateTransition<MyStatefulType, MyStates, RootStatefulTyoe, RootStatefulTypeStates, StateMachineType>[]
                    {
                        new MyStates.On((ss,state) => ss),
                        new MyStates.Off((ss,state) => ss),
                    },


```


*Create your state machine*

```C#

	var myStateMachine = new StateMachine<MyStatefulType, MyStates, RootStatefulTyoe, RootStatefulTypeStates, StateMachineType>(
		myTransitions, initialState:new SavedSearchState.Collapsed(), childStateMachines: null, parentStateMachines: null);

```

*Perform a state transition*

```C#

	myStateMachine.PerformTransition(myStateMachine, new MyStates.On());


```