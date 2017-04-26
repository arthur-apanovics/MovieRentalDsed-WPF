using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRentalDsed_WPF.Delegates
{
    public class DatabaseChangedDelegates
    {
        public delegate void MovieDatabaseChangedHandler(object sender, DatabaseChangedEventArgs args);

        public delegate void CustomerDatabaseChangedHandler(object sender, DatabaseChangedEventArgs args);
    }
}
