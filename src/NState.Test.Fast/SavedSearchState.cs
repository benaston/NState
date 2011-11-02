namespace NState.Test.Fast
{
    public class SavedSearchState : LucidUIState
    {
        public class Expanded : SavedSearchState
        {
            public Expanded()
            {
                Name = "Expanded";
                Description = "";
            }
        }

        public class Collapsed : SavedSearchState
        {
            public Collapsed()
            {
                Name = "Collapsed";
                Description = "";
            }
        }
    }
}