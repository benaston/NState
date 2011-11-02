namespace NState.Test.Fast
{
    using System;

    [Serializable]
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