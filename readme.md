NState
=====

A simple state machine for .NET.

If you like NState, please let others know by <a href="https://twitter.com/share?text=Check%20out%20NState%2C%20a%20simple%20.NET%20state%20machine.%20%23nstate%20%40benastontweet&url=https%3A%2F%2Fgithub.com%2Fbenaston%2FNState" target="_blank">sending a tweet</a> (opens a form in a new window to send a tweet).

Example of use:

```C#

	//arrange
	var transitions = new IStateTransition<BugState, TransitionActionStatus>[]
				         {
				             new BugTransition.Open(),
				             new BugTransition.Assign(new BugTransitionAction.Assign()),
				             new BugTransition.Defer(new BugTransitionAction.Defer()),
				             new BugTransition.Resolve(new BugTransitionAction.Resolve()),
				             new BugTransition.Close(new BugTransitionAction.Close()),
				         };
	var myStateMachine = new StateMachine<BugState, TransitionActionStatus>("example",
                                                                      transitions,
                                                                      initialState: new BugState.Open());
	
	//act
	var bug = new Bug("my bug name", myStateMachine); //Bug type inherits from Stateful base type
	
	//assert
	Assert.That(bug.CurrentState, Is.TypeOf<BugState.Open>()); //true	
	
	//act
	bug.Assign("example@example.com", out transitionActionStatus); //triggers a transition of the state machine
	
	//assert
	Assert.That(bug.CurrentState, Is.TypeOf<BugState.Assigned>()); //true
	Assert.That(transitionActionStatus, Is.EqualTo(TransitionActionStatus.Success)); //true
	Assert.That(bug.AssigneeEmail, Is.EqualTo("example@example.com")); //true
	
	//act
	var json = myStateMachine.ToJson();
	var myDeserializedStateMachine = new StateMachine<BugState, TransitionActionStatus>("example",
                                                                                         _transitions,
                                                                                         initialState: new BugState.Open());
	myDeserializedStateMachine.InitializeFromJson(json);
	
	//assert
	Assert.That(myDeserializedStateMachine.CurrentState, Is.TypeOf<BugState.Assigned>()); //true

```

Features:
--------

 - easy construction of trees of interdependent or orthogonal state machines
 - supports making domain objects stateful
 - trivial state machine tree persistence and retrieval to/from JSON
 - transition conditions, exit and entry actions
 - transition actions are fully-fledged types enabling use of dependency injection to maintain clean, testable code even for complex behavior
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

	public class Bug : Stateful<BugState, TransitionActionStatus>
	{
		public Bug(string title, IStateMachine<BugState, TransitionActionStatus> stateMachine) : base(stateMachine)
		{
			Title = title;
		}
		
		public string Title { get; set; }
		
		public string AssigneeEmail { get; set; }
		
		public string ClosedByName { get; set; }
		
		public Bug Open(out TransitionActionStatus transitionActionStatus)
		{
			return TriggerTransition(this, new BugState.Open(), out transitionActionStatus);
		}
        
		public Bug Assign(string assigneeEmail, out TransitionActionStatus transitionActionStatus)
		{
			dynamic dto = new ExpandoObject();
			dto.AssigneeEmail = assigneeEmail;

			return TriggerTransition(this, new BugState.Assigned(), out transitionActionStatus, dto);
		}
		
		public void Defer(out TransitionActionStatus transitionActionStatus)
		{
			return TriggerTransition(this, new BugState.Deferred(), out transitionActionStatus);
		}
		
		public void Resolve(out TransitionActionStatus transitionActionStatus)
		{
			return TriggerTransition(this, new BugState.Resolved(), out transitionActionStatus);
		}
		
		public Bug Close(string closedByName, out TransitionActionStatus transitionActionStatus)
		{
			dynamic dto = new ExpandoObject();
			dto.ClosedByName = closedByName;

			return TriggerTransition(this, new BugState.Closed(), out transitionActionStatus, dto);
		}
	}

```

**3. Define your transitions**

```C#

	public partial class BugTransition
	{
		public class Resolve : StateTransition<BugState, TransitionActionStatus>
		{
		    public Resolve(BugTransitionAction.Resolve transitionAction)
		        : base(transitionAction: transitionAction) { }
		
		    public override BugState[] StartStates
		    {
		        get { return new[] { new BugState.Assigned(), }; }
		    }
		
		    public override BugState[] EndStates
		    {
		        get { return new[] { new BugState.Resolved(), }; }
		    }
		}
		
		public class Assign : StateTransition<BugState, TransitionActionStatus>
		{
			//...
		}
		
		//etc.
	}	

```

**4. Define any transition actions**

```C#

	public partial class BugTransitionAction
	{
		public class Assign : TransitionAction<BugState, TransitionActionStatus>
		{
		    public override TransitionActionStatus Run(BugState targetState,
		                                            IStateMachine<BugState, TransitionActionStatus> stateMachine,
		                                            dynamic statefulObject, dynamic dto = null)
		    {
		        if (dto == null)
		        {
		            throw new ArgumentNullException("dto");
		        }
		
		        if (dto.AssigneeEmail == null)
		        {
		            throw new Exception("AssigneeEmail not supplied.");
		        }
		
		        statefulObject.AssigneeEmail = dto.AssigneeEmail;
		
		        return TransitionActionStatus.Success;
		    }
		}
		
		//etc.
	}

```

**5. Instantiate your state machine**


```C#

	//...
	
	var transitions = new IStateTransition<BugState, TransitionActionStatus>[]
				{
					new BugTransition.Open(),
					new BugTransition.Assign(new BugTransitionAction.Assign()),
					new BugTransition.Defer(new BugTransitionAction.Defer()),
					new BugTransition.Resolve(new BugTransitionAction.Resolve()),
					new BugTransition.Close(new BugTransitionAction.Close()),
				};	
	
	_stateMachine = new StateMachine<BugState, TransitionActionStatus>("example",
                                                                  transitions,
                                                                  initialState: new BugState.Open());
	
	//...

```


**6. Work with your stateful object**


```C#

	//arrange
	var bug = new Bug("my bug name", _stateMachine);	
	
	//act
	bug.Assign("example@example.com", out transitionActionStatus);
	
	//assert
	Assert.That(bug.CurrentState, Is.TypeOf(BugState.Assigned)); //true
	Assert.That(transitionActionStatus, Is.EqualTo(TransitionActionStatus.Success)); //true
	Assert.That(bug.AssigneeEmail, Is.EqualTo("example@example.com")); //true

```

**7. Persist and restore your state machine**


```C#

	//...
	
	var json = _myStateMachine.ToJson();
	//send json to Couch or somewhere...
	
	//later...
	
	var myStateMachine = new StateMachine<BugState, TransitionActionStatus>("example",
                                                                         _transitions,
                                                                         initialState: new BugState.Open());
	myStateMachine.InitializeFromJson(json);
	
	//continue where you left off...

```

How to build and/or run the tests:
--------

1. Run `/build/build.bat`
1. Type in the desired option
1. Hit return

License & Copyright
--------

This software is released under the GNU Lesser GPL. It is Copyright 2013, Ben Aston. I may be contacted at ben@bj.ma.

How to Contribute
--------

Pull requests including bug fixes, new features and improved test coverage are welcomed. Please do your best, where possible, to follow the style of code found in the existing codebase.
