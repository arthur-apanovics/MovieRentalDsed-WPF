using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Models;

namespace MovieDatabase
{
    public class UpdateTables
    {
        public void UpdateMovie(MovieModel movie)
        {
            var database = new DatabaseOperations();
            database.UpdatetMovieInTable(movie);
        }
    }
}
