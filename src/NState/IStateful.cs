﻿// Copyright 2011, Ben Aston (ben@bj.ma.)
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
	using System.Dynamic;

	/// <summary>
	/// 	Responsible for defining the interface for types that may be used inside with the StateMachine type (akin to workflow.)
	/// </summary>
	public interface IStateful<TStatefulObject, TState>
		where TStatefulObject : Stateful<TStatefulObject, TState>
		where TState : State
	{
		IStateMachine<TState> StateMachine { get; set; }

		TState CurrentState { get; }

		TExpectedReturn TriggerTransition<TExpectedReturn>(TExpectedReturn statefulObject, TState targetState,
		                                                   ExpandoObject dto);
	}
}