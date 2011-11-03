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
	
	//NOTE: BA; that this transitional functionality defined in lambdas solely for clarity here.
	//I am considering alternative approaches.
	var myTransitions = new IStateTransition<Bug, BugState>[]
				{
					new BugTransition.Open((bug,state,args) => ss),
					new BugTransition.Assign((bug,state,args) => { bug.AssigneeEmail = args;
										       _emailSender.SendEmail(bug.AssigneeEmail, "Bug assigned to you."); 
										       return ss; }),
					new BugTransition.Defer((bug,state,args) =>  { _emailSender.SendEmail(bug.AssigneeEmail, "You're off the hook."); 
										       bug.AssigneeEmail = String.Empty;
										       return ss; }),
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
		
		public string AssigneeEmail { get; set; }
		
		public void Assign(string assigneeEmail)
		{
			stateMachine.PerformTransition(this, BugState.Assigned, assigneeEmail);
		}    
		
		public void Defer()
		{
			stateMachine.PerformTransition(this, BugState.Deferred);
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