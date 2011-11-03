NState
=====

Simple state machine for .NET.

How to use:
--------

**0. Get it**

```shell
	nuget install nfeature
```


**1. Create some state transitions**

```C#
	var myTransitions = new IStateTransition<MyStatefulType, MyState, StateMachineType>[]
				{
					new MyState.On((ss,state) => ss),
					new MyState.Off((ss,state) => ss),
				};


```


**2. Create your state machine**

```C#

	var myStateMachine 
		= new StateMachine<MyStatefulType, MyState, StateMachineType>(myTransitions, initialState:new SavedSearchState.Collapsed());


```

**3. Perform a state transition**

```C#

	myStateMachine.PerformTransition(new MyState.On());


```