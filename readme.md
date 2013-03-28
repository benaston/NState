NState
=====

A simple state machine for .NET.

Example of use:

```C#

	var bugTransitions = new IStateTransition<BugState, TransitionStatus>[]
				         {
				             new BugTransition.Open(),
				             new BugTransition.Assign(new BugTransitionAction.Assign()),
				             new BugTransition.Defer(new BugTransitionAction.Defer()),
				             new BugTransition.Resolve(new BugTransitionAction.Resolve()),
				             new BugTransition.Close(new BugTransitionAction.Close()),
				         };
	var myStateMachine = new StateMachine<BugState, TransitionStatus>("example",
                                                                      bugTransitions,
                                                                      initialState: new BugState.Open());
	var bug = new Bug("my bug name", myStateMachine); //Bug type inherits from Stateful base type
	
	Assert.That(bug.CurrentState, Is.TypeOf<BugState.Open>()); //true	
	
	bug.Assign("example@example.com", out transitionStatus); //triggers a transition of the state machine
	
	Assert.That(bug.CurrentState, Is.TypeOf<BugState.Assigned>()); //true
	Assert.That(transitionStatus, Is.EqualTo(TransitionStatus.Success)); //true
	
	var json = myStateMachine.ToJson();
	var myDeserializedStateMachine = new StateMachine<BugState, TransitionStatus>("example",
                                                                                         _transitions,
                                                                                         initialState: new BugState.Open());
	myDeserializedStateMachine.InitializeFromJson(json);
	
	Assert.That(myDeserializedStateMachine.CurrentState, Is.TypeOf<BugState.Assigned>()); //true

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
		public Bug(string title, IStateMachine<BugState> stateMachine) : base(stateMachine)
										 {
											Title = title;
										 }
		
		public string Title { get; set; }
		
		public string AssigneeEmail { get; set; }
		
		public string ClosedByName { get; set; }
		
		public Bug Open(out TransitionStatus transitionStatus)
		{
			return TriggerTransition(this, new BugState.Open(), out transitionStatus);
		}
        
		public Bug Assign(string assigneeEmail, out TransitionStatus transitionStatus)
		{
			dynamic dto = new ExpandoObject();
            		dto.AssigneeEmail = assigneeEmail;

            		return TriggerTransition(this, new BugState.Assigned(), out transitionStatus, dto);
		}
		
		public void Defer(out TransitionStatus transitionStatus)
		{
			return TriggerTransition(this, new BugState.Deferred(), out transitionStatus);
		}
		
		public void Resolve(out TransitionStatus transitionStatus)
		{
			return TriggerTransition(this, new BugState.Resolved(), out transitionStatus);
		}
		
		public Bug Close(string closedByName, out TransitionStatus transitionStatus)
		{
			dynamic dto = new ExpandoObject();
            		dto.ClosedByName = closedByName;

            		return TriggerTransition(this, new BugState.Closed(), out transitionStatus, dto);
		}
	}

```

**3. Define your transitions**

```C#

	public partial class BugTransition
	{
		public class Resolve : StateTransition<BugState, TransitionStatus>
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
		
		public class Assign : StateTransition<BugState, TransitionStatus>
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
		public class Assign : TransitionAction<BugState, TransitionStatus>
		{
		    public override TransitionStatus Run(BugState targetState,
		                                            IStateMachine<BugState, TransitionStatus> stateMachine,
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
		
		        statefulObject.Bug.AssigneeEmail = dto.AssigneeEmail;
		
		        return TransitionStatus.Success;
		    }
		}
		
		//etc.
	}

```

**5. Instantiate your state machine**


```C#

	//...
	
	var transitions = new IStateTransition<BugState, TransitionStatus>[]
				{
					new BugTransition.Open(),
					new BugTransition.Assign(new BugTransitionAction.Assign()),
					new BugTransition.Defer(new BugTransitionAction.Defer()),
					new BugTransition.Resolve(new BugTransitionAction.Resolve()),
					new BugTransition.Close(new BugTransitionAction.Close()),
				};	
	
	_stateMachine = new StateMachine<BugState, TransitionStatus>("example",
                                                                  transitions,
                                                                  initialState: new BugState.Open());
	
	//...

```


**6. Work with your stateful object**


```C#

	//arrange
	var bug = new Bug("my bug name", _stateMachine);	
	
	//act
	bug.Assign("example@example.com");
	
	//assert
	Assert.That(bug.CurrentState, Is.TypeOf(BugState.Assigned)); //true
	Assert.That(transitionStatus, Is.EqualTo(TransitionStatus.Success)); //true
	Assert.That(bug.AssigneeEmail, Is.EqualTo("example@example.com")); //true

```

**7. Persist and restore your state machine**


```C#

	//...
	
	var json = _myStateMachine.ToJson();
	//send json to Couch or somewhere...
	
	//later...
	
	var myStateMachine = new StateMachine<BugState, TransitionStatus>("example",
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

This software is released under the GNU Lesser GPL. It is Copyright 2012, Ben Aston. I may be contacted at ben@bj.ma.

How to Contribute
--------

Pull requests including bug fixes, new features and improved test coverage are welcomed. Please do your best, where possible, to follow the style of code found in the existing codebase.
