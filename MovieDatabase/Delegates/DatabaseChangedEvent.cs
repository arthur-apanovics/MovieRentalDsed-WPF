using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Delegates
{
    public class DatabaseChangedEvent
    {
        public event EventHandler<DatabaseChangedEventArgs> DatabaseHasChanged;
    }
}
