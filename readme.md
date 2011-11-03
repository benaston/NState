NState
=====

Simple state machine for .NET. This software is NOT production ready.

How to use:
--------

**0. Get it**

```shell
	nuget install nstate
```


**1. Create some state transitions**

```C#

	var myTransitions = new IStateTransition<MyStatefulType, MyState, StateMachineType>[]
				{
					new MyState.Off((ss,state) => ss),
					new MyState.On((ss,state) => ss),
				};


```


**2. Create your state machine**

```C#

	var myStateMachine 
		= new StateMachine<MyStatefulType, MyState, StateMachineType>(myTransitions, initialState:new MyState.Off());




**3. Create your stateful type**

```C#

	public class MyStatefulType : Stateful<MyStatefulType, MyState, StateMachineType>
	{
		public MyStatefulType(IStateMachine<MyStatefulType, MyState, StateMachineType> stateMachine)
			: base(stateMachine) {}

		//...
	}


``````


    
**4. Perform a state transition**

```C#

	var myStatefulType = new MyStatefulType(myStateMachine).PerformTransition(new MyState.On());


```