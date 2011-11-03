NState
=====

Simple state machine for .NET.

Usage:
--------

PLEASE NOTE: this project is unfinished and I realise that the API is currently pretty horrible.

*Create some state transitions*

```C#
	var myTransitions = new IStateTransition<MyStatefulType, MyState, StateMachineType>[]
				{
					new MyState.On((ss,state) => ss),
					new MyState.Off((ss,state) => ss),
				};


```


*Create your state machine*

```C#

	var myStateMachine 
		= new StateMachine<MyStatefulType, MyState, StateMachineType>(myTransitions, initialState:new SavedSearchState.Collapsed());


```

*Perform a state transition*

```C#

	myStateMachine.PerformTransition(new MyState.On());


```