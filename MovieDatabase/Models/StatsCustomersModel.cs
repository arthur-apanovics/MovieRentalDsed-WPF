using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Models
{
    public class StatsCustomersModel
    {
        public string FullName { get; set; }
        public int TotalRentals { get; set; }

        public StatsCustomersModel(string title, int totalRentals)
        {
            FullName = title;
            TotalRentals = totalRentals;
        }
    }
}
