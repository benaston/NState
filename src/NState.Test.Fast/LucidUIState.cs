namespace NState.Test.Fast
{
    public class LucidUIState : State
    {
        public class Active : LucidUIState
        {
            public Active()
            {
                Name = "Active";
                Description = "State changes may occur.";
            }
        }

        public class Paused : LucidUIState
        {
            public Paused()
            {
                Name = "Paused";
                Description = "State changes may not occur.";
            }
        }
    }
}