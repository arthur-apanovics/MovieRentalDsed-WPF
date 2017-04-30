using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class RentedMovieData
    {
        public List<RentedMovieModel> RentedMovies { get; set; }

        public RentedMovieData()
        {
            DatabaseOperations database = new DatabaseOperations();
            RentedMovies = database.GetAllRentedMovies().Result;
        }
    }
}
