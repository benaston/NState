﻿// Copyright 2011, Ben Aston (ben@bj.ma.)
// 
// This file is part of NState.
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
// along with NState.  If not, see <http://www.gnu.org/licenses/>.

namespace NState.Test.Fast.UserInterfaceExample
{
	public class SearchArea : Stateful<UIRoot, LucidState>
	{
		public SearchArea(IStateMachine<LucidState> stateMachine)
			: base(stateMachine) {}

		public SearchArea Hide() {
			TriggerTransition(this, new SearchAreaState.Hidden());

			return this;
		}

		public SearchArea Show() {
			TriggerTransition(this, new SearchAreaState.Visible());

			return this;
		}
	}
}