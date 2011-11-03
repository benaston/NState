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
            }
        }

        public class Expanded : AccountTabState
        {
            public Expanded()
            {
            }
        }
    }
}