// Copyright 2011, Ben Aston (ben@bj.ma.)
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

namespace NState
{
	public abstract class State
	{
		private string _name;

		public string Name {
			get { return _name ?? GetType().AssemblyQualifiedName; //assembly qualified for (de)serialization
			}

			set { _name = value; }
		}

		public static bool operator ==(State a, State b) {
			return (a.Equals(b));
		}

		public static bool operator !=(State a, State b) {
			return !(a.Equals(b));
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (State)) return false;

			return Equals((State) obj);
		}

		public bool Equals(State other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Name, Name);
		}

		public override int GetHashCode() {
			unchecked {
				return ((Name != null ? Name.GetHashCode() : 0)*397);
			}
		}

		public virtual void EntryAction(dynamic dto) {}

		public virtual void ExitAction(dynamic dto) {}
	}
}