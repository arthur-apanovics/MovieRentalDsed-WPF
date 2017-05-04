using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Models
{
    public class StatsMoviesModel
    {
        public string MovieTitle { get; set; }
        public int TotalRentals { get; set; }

        public StatsMoviesModel(string name, int totalRentals)
        {
            MovieTitle = name;
            TotalRentals = totalRentals;
        }
    }
}
