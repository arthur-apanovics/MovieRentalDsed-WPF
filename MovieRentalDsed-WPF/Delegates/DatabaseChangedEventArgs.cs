using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRentalDsed_WPF.Delegates
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
