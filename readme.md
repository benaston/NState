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

	public class Bug : Stateful<BugTracker, MyAppState>
	{
		public Bug(string title, IStateMachine<BugTracker, MyAppState> stateMachine)
		: base(stateMachine)
		{
		Title = title;
		}
		
		public string Title { get; set; }
		
		public string AssigneeEmail { get; set; }
		
		public string ClosedByName { get; set; }
		
		public void Assign(string assigneeEmail)
		{
			PerformTransition<Bug>(new BugState.Assigned(), new { AssigneeEmail = assigneeEmail });
		}
		
		public void Defer()
		{
			PerformTransition<Bug>(new BugState.Deferred());
		}
		
		public void Resolve()
		{
			PerformTransition<Bug>(new BugState.Resolved());
		}
		
		public void Close(string closedByName)
		{
			PerformTransition<Bug>(new BugState.Closed(), new { ClosedByName = closedByName });
		}
	}

```

**3. Define your transitions**

```C#

	[Serializable]
	public class BugTransition
	{
		[Serializable]
		public class Assign : StateTransition<Bug, MyAppState>
		{
			public Assign(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }
			
			public override MyAppState[] StartStates
			{
				get { return new BugState[] {new BugState.Open(), new BugState.Assigned(),}; }
			}
			
			public override MyAppState[] EndStates
			{
				get { return new[] {new BugState.Assigned(),}; }
			}
		}
		
		[Serializable]
		public class Close : StateTransition<Bug, MyAppState>
		{
			public Close(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }
			
			public override MyAppState[] StartStates
			{
			get { return new BugState[] { new BugState.Resolved() }; }
			}
			
			public override MyAppState[] EndStates
			{
			get { return new[] { new BugState.Closed(), }; }
			}
		}
			
		[Serializable]
		public class Defer : StateTransition<Bug, MyAppState>
		{
			public Defer(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }
			
			public override MyAppState[] StartStates
			{
			get { return new BugState[] { new BugState.Open(), new BugState.Assigned() }; }
			}
			
			public override MyAppState[] EndStates
			{
			get { return new[] { new BugState.Deferred(), }; }
			}
		}
		
		[Serializable]
		public class Open : StateTransition<Bug, MyAppState>
		{
			public Open(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }
			
			public override MyAppState[] StartStates
			{
			get { return new[] {new BugState.Closed(),}; }
			}
			
			public override MyAppState[] EndStates
			{
			get { return new[] {new BugState.Open(),}; }
			}
		}
		
		[Serializable]
		public class Resolve : StateTransition<Bug, MyAppState>
		{
			public Resolve(Action<MyAppState, dynamic> transitionFunction = null) : base(transitionFunction) { }
			
			public override MyAppState[] StartStates
			{
				get { return new[] { new BugState.Assigned(), }; }
			}
			
			public override MyAppState[] EndStates
			{
				get { return new[] { new BugState.Resolved(), }; }
			
			}
		}
	}

```

**4. Define your transition functions (optional logic run within the transitions)**

```C#

	public class BugHelper
	{
		public static void Assign(MyAppState state, dynamic args)
		{
			args.StatefulObject.AssigneeEmail = args.AssigneeEmail;
		}
		
		public static void Defer(MyAppState state, dynamic args)
		{
			args.StatefulObject.AssigneeEmail = String.Empty;
		}
		
		public static void Resolve(MyAppState state, dynamic args)
		{
			args.StatefulObject.AssigneeEmail = String.Empty;
		}
		
		public static void Close(MyAppState state, dynamic args)
		{
			args.StatefulObject.ClosedByName = args.ClosedByName;
		}
	}

```

**5. Instantiate your state machine**


```C#

	//...
	
	var transitions = new IStateTransition<MyAppState>[]
				{
					new BugTransition.Open(),
					new BugTransition.Assign(BugHelper.Assign),
					new BugTransition.Defer(BugHelper.Defer),
					new BugTransition.Resolve(BugHelper.Resolve),
					new BugTransition.Close(BugHelper.Close),
				};
	
	var parentTransitions = new IStateTransition<MyAppState>[]
	{
		new BugTrackerTransition.SetAlight(),
		new BugTrackerTransition.Extinguish(),
	};
	
	var myStateMachine = new StateMachine<BugTracker, MyAppState>(transitions, startState:new BugState.Open());
	
	//...

```


**6. Work with you stateful object**


```C#

	var bug = new Bug("bug1", _stateMachine);	
	bug.Assign("example@example.com");
	
	Assert.That(bug.CurrentState == new BugState.Assigned());

```



