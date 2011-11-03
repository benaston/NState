NState

=====

Simple state machine for .NET. This software is NOT production ready.

How to use:
--------

**0. Get it**

```shell
	nuget install nstate
```

**1. Create some states**

```C#

	[Serializable]
	public abstract class BugState : State
	{
		public class Assigned : BugState {}

 		public class Closed : BugState {}

 		public class Deferred : BugState {}

  		public class Open : BugState {}

 		public class Resolved : BugState {}
	}
    

```

**2. Define your stateful type**

```C#

	[Serializable]
	public class Bug : Stateful<Bug, BugState>
	{
		public Bug(string title, IStateMachine<Bug, BugState> stateMachine) : base(stateMachine)
		{
			Title = title;
		}
		
		public string Title { get; set; }
		
		public string AssigneeEmail { get; set; }
		
		public string ClosedByName { get; set; }
		
		public void Assign(string assigneeEmail)
		{
			TransitionTo(new BugState.Assigned(), new {AssigneeEmail = assigneeEmail});
		}
		
		public void Defer()
		{
			TransitionTo(new BugState.Deferred());
		}
		
		public void Resolve()
		{
			TransitionTo(new BugState.Resolved());
		}
		
		public void Close(string closedByName)
		{
			TransitionTo(new BugState.Closed(), new {ClosedByName = closedByName});
		}
	}


```

**3. Define your transitions**

```C#

	[Serializable]
	public class BugTransition
	{
		[Serializable]
		public class Assign : StateTransition<Bug, BugState>
		{
			public Assign(Func<Bug, BugState, dynamic, Bug> transitionFunction) : base(transitionFunction) {}
	
			public override BugState[] StartStates
			{
				get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Assigned(),}; }
			}
		}
	
		[Serializable]
		public class Close : StateTransition<Bug, BugState>
		{
			public Close(Func<Bug, BugState, dynamic, Bug> transitionFunction) : base(transitionFunction) {}
		
			public override BugState[] StartStates
			{
				get { return new[] {new BugState.Resolved(),}; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Closed(),}; }
			}
		}
		
		[Serializable]
		public class Defer : StateTransition<Bug, BugState>
		{
			public Defer(Func<Bug, BugState, dynamic, Bug> transitionFunction) : base(transitionFunction) {}
		
			public override BugState[] StartStates
			{
				get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
			}
		
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Deferred(),}; }
			}
		}
		
		[Serializable]
		public class Open : StateTransition<Bug, BugState>
		{
			public override BugState[] StartStates
			{
				get { return new[] {new BugState.Closed(),}; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Open(),}; }
			}
		}
	
		[Serializable]
		public class Resolve : StateTransition<Bug, BugState>
		{
			public Resolve(Func<Bug, BugState, dynamic, Bug> transitionFunction) : base(transitionFunction) {}
		
			public override BugState[] StartStates
			{
				get { return new[] {new BugState.Assigned(),}; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Resolved(),}; }
			}
		}
	}


```

**4. Define your transition functions (optional logic run within the transitions)**

```C#

	public class BugHelper
	{
		public static Bug Assign(Bug bug, BugState state, dynamic args)
		{
			bug.AssigneeEmail = args.AssigneeEmail;
		
			return bug;
		}
		
		public static Bug Defer(Bug bug, BugState state, dynamic args)
		{
			bug.AssigneeEmail = String.Empty;
		
			return bug;
		}
		
		public static Bug Resolve(Bug bug, BugState state, dynamic args)
		{
			bug.AssigneeEmail = String.Empty;
		
			return bug;
		}
		
		public static Bug Close(Bug bug, BugState state, dynamic args)
		{
			bug.ClosedByName = args.ClosedByName;
		
			return bug;
		}
	}

```

**5. Instantiate your state machine**


```C#

	//...
	
	var transitions = new IStateTransition<Bug, BugState>[]
	{
		new BugTransition.Open(),
		new BugTransition.Assign(BugHelper.Assign),
		new BugTransition.Defer(BugHelper.Defer),
		new BugTransition.Resolve(BugHelper.Resolve),
		new BugTransition.Close(BugHelper.Close),
	};
	
	_stateMachine = new StateMachine<Bug, BugState>(transitions, startState: new BugState.Open());
	
	//...

```


**6. Work with you stateful object**


```C#

	var bug = new Bug("bug1", _stateMachine);	
	bug.Assign("example@example.com");
	Assert.That(bug.CurrentState == new BugState.Assigned());

```



