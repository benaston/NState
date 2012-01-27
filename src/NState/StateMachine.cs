// Copyright 2011, Ben Aston (ben@bj.ma.)
// 
// This file is part of NFeature.
// 
// NFeature is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NFeature is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NFeature.  If not, see <http://www.gnu.org/licenses/>.

namespace NState
{
	using System;
	using System.Collections.Generic;
	using System.Dynamic;
	using System.Linq;
	using System.Transactions;
	using NBasicExtensionMethod;
	using NSure;
	using Newtonsoft.Json;
	using ArgumentException = NHelpfulException.FrameworkExceptions.ArgumentException;

	/// <summary>
	/// 	Enables specification of valid state changes to be applied to object instances.
	/// </summary>
	public class StateMachine<TState> :
		IStateMachine<TState>
		where TState : State
	{
		private Dictionary<string, IStateMachine<TState>> _children = new Dictionary<string, IStateMachine<TState>>();
		public StateMachine() {} //for deserialization

		public StateMachine(string name,
		                    IEnumerable<IStateTransition<TState>> stateTransitions,
		                    TState initialState,
		                    TState finalState = null,
		                    IStateMachine<TState> parentStateMachine = null,
		                    bool permitSelfTransition = true,
		                    bool bypassTransitionBehaviorForSelfTransition = true) //if permitted
		{
			Ensure.That<ArgumentException>(name.IsNotNullOrWhiteSpace(), "name not supplied.")
				.And<ArgumentException>(stateTransitions.IsNotNull(), "stateTransitions not supplied.")
				.And<ArgumentException>(initialState.IsNotNull(), "initialState not supplied");

			Name = name;
			StateTransitions = stateTransitions;
			InitialState = initialState;
			FinalState = finalState;
			Parent = parentStateMachine;
			PermitSelfTransition = permitSelfTransition;
			BypassTransitionBehaviorForSelfTransition = bypassTransitionBehaviorForSelfTransition;
			if (parentStateMachine != null)
			{
				Parent.Children.Add(Name, this);
			}
			CurrentState = initialState;
		}

		public TState FinalState { get; set; }
		public bool PermitSelfTransition { get; set; }

		public bool BypassTransitionBehaviorForSelfTransition { get; set; }

		public string Name { get; set; }

		public IEnumerable<IStateTransition<TState>> StateTransitions { get; protected set; }

		public TState InitialState { get; set; }

		public IStateMachine<TState> Parent { get; set; }

		public Dictionary<string, IStateMachine<TState>> Children
		{
			get { return _children; }
			set { _children = value; }
		}

		public TState CurrentState { get; set; }

		public Dictionary<DateTime, IStateTransition<TState>> History { get; set; }

		/// <summary>
		/// 	NOTE 1: http://cs.hubfs.net/blogs/hell_is_other_languages/archive/2008/01/16/4565.aspx
		/// </summary>
		public void TriggerTransition(TState targetState,
		                              dynamic args = default(dynamic))
		{
			Ensure.That<ArgumentException>(targetState != null, "targetState not supplied.");

			try
			{
				if (CurrentState == targetState && !PermitSelfTransition)
				{
					throw new Exception(); //refactor to refine exception
				}

				if (CurrentState == FinalState)
				{
					throw new Exception(); //refactor to refine exception
				}

				if ((CurrentState != targetState || (CurrentState == targetState && !BypassTransitionBehaviorForSelfTransition)))
				{
					var matches = StateTransitions.Where(t =>
					                                     t.InitialStates.Where(s => s == CurrentState).Any() &&
					                                     t.EndStates.Where(e => e == targetState).Any());
					if (matches.Any())
					{
						using (var t = new TransactionScope())
							//this could be in-memory transactionalised using the memento pattern, or information could be sent to F# (see NOTE 1)
						{
							OnRaiseBeforeEveryTransition();
							CurrentState.ExitAction(args);
							matches.First().TransitionAction(targetState, this, args);
							targetState.EntryAction(args);
							CurrentState = targetState;
							OnRaiseAfterEveryTransition();
							t.Complete();
						}
					}
					else
					{
						if (Parent == null)
						{
							throw new Exception(); //to be caught below, refactor
						}

						Parent.TriggerTransition(targetState, args);
					}
				}
			}
			catch (Exception e)
			{
				throw new InvalidStateTransitionException<TState>(CurrentState, targetState, innerException: e);
			}
		}

		public event EventHandler RaiseBeforeEveryTransitionEvent;

		public event EventHandler RaiseAfterEveryTransitionEvent;

		// Wrap event invocations inside a protected virtual method
		// to allow derived classes to override the event invocation behavior
		protected virtual void OnRaiseBeforeEveryTransition()
		{
			// Make a temporary copy of the event to avoid possibility of
			// a race condition if the last subscriber unsubscribes
			// immediately after the null check and before the event is raised.
			var handler = RaiseBeforeEveryTransitionEvent;

			// Event will be null if there are no subscribers
			if (handler != null)
			{
				// Format the string to send inside the CustomEventArgs parameter
				//e.Message += String.Format(" at {0}", DateTime.Now.ToString());

				// Use the () operator to raise the event.
				handler(this, new EventArgs());
			}
		}

		protected virtual void OnRaiseAfterEveryTransition()
		{
			var handler = RaiseAfterEveryTransitionEvent;

			if (handler != null) // Event will be null if there are no subscribers
			{
				handler(this, new EventArgs());
			}
		}

		public string SerializeToJsonDto()
		{
			var dto = StateMachineSerializationHelper.SerializeToDto(this, new ExpandoObject());
			var s = JsonConvert.SerializeObject(dto, Formatting.Indented,
			                                    new JsonSerializerSettings
			                                    	{ObjectCreationHandling = ObjectCreationHandling.Replace});

			return s;
		}

		/// <summary>
		/// 	Not in constructor because SM tree may not be completely initialized by constructor in current implementation.
		/// </summary>
		public IStateMachine<TState> InitializeWithJson(string json)
		{
			return StateMachineSerializationHelper.InitializeWithDto(this, JsonConvert.DeserializeObject
			                                                               	(json));
		}
	}
}