namespace MovieDatabase.Delegates
{
    public class DatabaseChangedDelegates
    {
        public delegate void DatabaseChangedHandler(object sender, DatabaseChangedEventArgs args);
    }
}
