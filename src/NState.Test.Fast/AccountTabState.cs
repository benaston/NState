namespace NState.Test.Fast
{
    using System;

    [Serializable]
    public class AccountTabState : LucidUIState
    {
        public class Collapsed : AccountTabState
        {
            public Collapsed()
            {
                Name = "Collapsed";
                Description = "";
            }
        }

        public class Expanded : AccountTabState
        {
            public Expanded()
            {
                Name = "Expanded";
                Description = "";
            }
        }
    }
}