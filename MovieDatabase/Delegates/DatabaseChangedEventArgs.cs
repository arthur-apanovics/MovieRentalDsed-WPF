using System;

namespace MovieDatabase.Delegates
{
    public class DatabaseChangedEventArgs : EventArgs
    {
        public DateTime Timestamp { get; set; }

        public DatabaseChangedEventArgs(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
