namespace NState.Test.Fast
{
    public class AccountTabState : LucidUIState
    {
        public class Expanded : AccountTabState
        {
            public Expanded()
            {
                Name = "Expanded";
                Description = "";
            }
        }

        public class Collapsed : AccountTabState
        {
            public Collapsed()
            {
                Name = "Collapsed";
                Description = "";
            }
        }
    }
}