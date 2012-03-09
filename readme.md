NState
=====

Simple state machine for .NET.

Example of use:

```C#

	//NOTE: state machine and transitions might be supplied by service locator in real-life
	var transitions = new IStateTransition<BugState>[] {
		new BugTransition.Open(),
		new BugTransition.Assign(BugTransitionAction.Assign),
		new BugTransition.Defer(BugTransitionAction.Defer),
		new BugTransition.Resolve(BugTransitionAction.Resolve),
		new BugTransition.Close(BugTransitionAction.Close),
	};
	var myStateMachine = new StateMachine<Bug, BugState>(transitions, initialState:new BugState.Open());	
	var bug = new Bug("my bug name", myStateMachine); //Bug type inherits from Stateful base type
	
	Assert.That(bug.CurrentState == new BugState.Open()); //true
	
	bug.Assign("example@example.com"); //triggers a transition of the state machine
	
	Assert.That(bug.CurrentState == new BugState.Assigned()); //true
	
	var json = myStateMachine.SerializeToJsonDto();
	var myDeserializedStateMachine = new StateMachine<Bug, BugState>(transitions, initialState:new BugState.Open());
	myDeserializedStateMachine.InitializeWithJson(json);

```

Features:
--------

 - easy construction of trees of interdependent or orthogonal state machines
 - supports making domain objects stateful
 - trivial state machine tree persistence and retrieval to/from JSON
 - transition conditions, exit and entry actions
 - transition actions with arbitrary arguments
 - initial and final state specification


How to use:
--------

**0. Get it**

```shell
	nuget install nstate
```

**1. Create some states**

```C#

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

	public class Bug : Stateful<Bug, BugState>
	{
		public Bug(string title, IStateMachine<BugState> stateMachine)
		: base(stateMachine)
		{
			Title = title;
		}
		
		public string Title { get; set; }
		
		public string AssigneeEmail { get; set; }
		
		public string ClosedByName { get; set; }
		
		public Bug Open()
		{
			return TriggerTransition(this, new BugState.Open());
		}
        
		public Bug Assign(string assigneeEmail)
		{
			dynamic args = new ExpandoObject();
			args.AssigneeEmail = assigneeEmail;
			
			return TriggerTransition(this, new BugState.Assigned(), args);
		}
		
		public void Defer()
		{
			TriggerTransition(this, new BugState.Deferred());
		}
		
		public void Resolve()
		{
			TriggerTransition(this, new BugState.Resolved());
		}
		
		public Bug Close(string closedByName)
		{
			dynamic args = new ExpandoObject();
			args.ClosedByName = closedByName;
		
			return TriggerTransition(this, new BugState.Closed(), args);
		}
	}

```

**3. Define your transitions**

```C#

	public class BugTransition
	{
		public class Assign : StateTransition<Bug, BugState>
		{
			public Assign(Action<BugState, IStateMachine<BugState>, dynamic> transitionAction = null) : base(transitionAction) { }
			
			public override BugState[] InitialStates
			{
				get { return new BugState[] {new BugState.Open(), new BugState.Assigned(), }; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Assigned(), }; }
			}
		}
		
		public class Close : StateTransition<Bug, BugState>
		{
			public Close(Action<BugState, IStateMachine<BugState>, dynamic> transitionAction = null) : base(transitionAction) { }
			
			public override BugState[] InitialStates
			{
				get { return new BugState[] { new BugState.Resolved() }; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] { new BugState.Closed(), }; }
			}
		}
			
		public class Defer : StateTransition<Bug, BugState>
		{
			public Defer(Action<BugState, IStateMachine<BugState>, dynamic> transitionAction = null) : base(transitionAction) { }
			
			public override BugState[] InitialStates
			{
				get { return new BugState[] { new BugState.Open(), new BugState.Assigned() }; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] { new BugState.Deferred(), }; }
			}
		}
		
		public class Open : StateTransition<Bug, BugState>
		{
			public Open(Action<BugState, IStateMachine<BugState>, dynamic> transitionAction = null) : base(transitionAction) { }
			
			public override BugState[] InitialStates
			{
				get { return new[] {new BugState.Closed(), }; }
			}
			
			public override BugState[] EndStates
			{
				get { return new[] {new BugState.Open(), }; }
			}
		}
		
		public class Resolve : StateTransition<Bug, BugState>
		{
			public Resolve(Action<BugState, IStateMachine<BugState>, dynamic> transitionAction = null) : base(transitionAction) { }
			
			public override MyAppState[] InitialStates
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

**4. Define any transition actions**

```C#

	public class BugTransitionAction
	{
		public static void Assign(BugState state, IStateMachine<BugState> stateMachine, dynamic args)
		{
			args.StatefulObject.AssigneeEmail = args.AssigneeEmail;
		}
		
		public static void Defer(BugState state, IStateMachine<BugState> stateMachine, dynamic args)
		{
			args.StatefulObject.AssigneeEmail = String.Empty;
		}
		
		public static void Resolve(BugState state, IStateMachine<BugState> stateMachine, dynamic args)
		{
			args.StatefulObject.AssigneeEmail = String.Empty;
		}
		
		public static void Close(BugState state, IStateMachine<BugState> stateMachine, dynamic args)
		{
			args.StatefulObject.ClosedByName = args.ClosedByName;
		}
	}

```

**5. Instantiate your state machine**


```C#

	//...
	
	var transitions = new IStateTransition<BugState>[]
				{
					new BugTransition.Open(),
					new BugTransition.Assign(BugTransitionAction.Assign),
					new BugTransition.Defer(BugTransitionAction.Defer),
					new BugTransition.Resolve(BugTransitionAction.Resolve),
					new BugTransition.Close(BugTransitionAction.Close),
				};	
	
	var myStateMachine = new StateMachine<Bug, BugState>(transitions, initialState:new BugState.Open());
	
	//...

```


**6. Work with your stateful object**


```C#

	var bug = new Bug("my bug name", _stateMachine);	
	bug.Assign("example@example.com");
	
	Assert.That(bug.CurrentState == new BugState.Assigned());

```

**7. Persist and restore your state machine**


```C#

	//...
	
	var json = _myStateMachine.SerializeToJsonDto();
	//send json to Couch or somewhere...
	
	//later...
	
	var myStateMachine = new StateMachine<Bug, BugState>(transitions, initialState:new BugState.Open());
	myStateMachine.InitializeWithJson(json);
	
	//continue where you left off...

```


License & Copyright
--------

This software is released under the GNU Lesser GPL. It is Copyright 2011, Ben Aston. I may be contacted at ben@bj.ma.

