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

namespace NState.Test.Fast.UserInterfaceExample
{
	public class SearchAreaTransitionAction
	{
		/// <summary>
		/// 	//when showing the search panel, ensure the working panel is in the home position?
		/// </summary>
		public static void Show(LucidState state,
		                        IStateMachine<LucidState> stateMachine,
		                        dynamic args) {
			//       search panel search tab         home panel         root
			stateMachine.Parent.Parent.Parent.Children["WorkingPanel"].TriggerTransition(
				new WorkingPanelState.SearchMode());
			stateMachine.Parent.Parent.Parent.Children["DetailsPanels"].TriggerTransition(
				new DetailsPanelsState.SearchMode());
		}
	}
}