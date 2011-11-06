namespace NState
{
    using System;

    [Serializable]
    public abstract class 
        State
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name ?? GetType().AssemblyQualifiedName; //assembly qualified for (de)serialization
            }

            set
            {
                _name = value;
            }
        }

        //public string Description { get; protected set; }

        public static bool operator ==(State a, State b)
        {
            return (a.Equals(b));
        }

        public static bool operator !=(State a, State b)
        {
            return !(a.Equals(b));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (State)) return false;

            return Equals((State) obj);
        }

        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);// && Equals(other.Description, Description);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397)
                       ;//(Description != null ? Description.GetHashCode() : 0);
            }
        }

        public virtual void EntryFunction(dynamic dto) {}

        public virtual void ExitFunction(dynamic dto) {}
    }
}