using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class StatsData
    {
        public List<StatsMoviesModel> TopMovieStats { get; set; }
        public List<StatsCustomersModel> TopCustomers { get; set; }

        public StatsData()
        {
            var database = new DatabaseOperations();

            TopMovieStats = database.GetStatsForMovies().Result;
            TopCustomers = database.GetStatsForCustomers().Result;
        }

    }
}
