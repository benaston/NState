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

*This example is taken from Stateless, another, (significantly better!) state machine for .NET*

```C#

	var _emailSender = new MyEmailSender(); //full definition left out for clarity
	
	var myTransitions = new IStateTransition<Bug, BugState>[]
				{
					new BugTransition.Open((bug,state,args) => ss),
					new BugTransition.Assign((bug,state,args) => { _emailSender.SendEmail(args.Assignee, "Bug assigned to you."); return ss; }),
					new BugTransition.Defer((bug,state,args) => ss),
					new BugTransition.Resolve((bug,state,args) => ss),
					new BugTransition.Close((bug,state,args) => ss),
				};

```


**2. Create your state machine**


```C#

	var myStateMachine 
		= new StateMachine<Bug, BugState>(myTransitions, initialState:new BugState.Open());

```


**3. Create your stateful type**


```C#

	public class Bug : Stateful<Bug, BugState>
	{
		public MyStatefulType(string title, IStateMachine<MyStatefulType, MyState> stateMachine)
			: base(stateMachine) 
		{
			Title = title;
		}

		public string Title { get; set; }
		
		public string Assignee { get; set; }
		
		public void Assign(string assignee)
		{
			Assignee = assignee;
			stateMachine.PerformTransition(this, BugState.Assigned);
		}    
		
		public void Defer()
		{
			stateMachine.PerformTransition(this, BugState.Deferred);
			Assignee = String.Empty;
		}    
		
		public void Resolve()
		{
			stateMachine.PerformTransition(this, BugState.Resolved);
		}
		
		public void Close()
		{
			stateMachine.PerformTransition(this, BugState.Closed);
		}    
	}

``````


    
**4. Perform a state transition**


```C#

	var myStatefulType = new MyStatefulType(myStateMachine); //state is "Off"
	myStatefulType = myStatefulType.PerformTransition(new MyState.On()); //state transitioned to "On"

```