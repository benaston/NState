// Copyright 2012, Ben Aston (ben@bj.ma).
// 
// This file is part of NState.
// 
// NState is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NState is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NState. If not, see <http://www.gnu.org/licenses/>.

namespace NState
{
	using System;
	using NHelpfulException;

	public class InvalidStateTransitionException<TState> : HelpfulException
		where TState : State
	{
		private const string DefaultMessage = "Unable to transition state from {0} to {1}.";

		public InvalidStateTransitionException(TState initialState,
																					 TState endState,
																					 string[] resolutionSuggestions = default (string[]),
																					 Exception innerException = default(Exception))
			: base(
				string.Format(DefaultMessage, initialState.Name, endState.Name),
				resolutionSuggestions,
				innerException) {}
	}
}