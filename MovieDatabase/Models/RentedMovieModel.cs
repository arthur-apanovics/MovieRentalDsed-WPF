using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Models
{
    public class RentedMovieModel
    {
        public int MovieId { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime DateReturned { get; set; }

        public RentedMovieModel(int movieId, int customerId, string fName, string lName, DateTime issued, DateTime returned)
        {
            MovieId = movieId;
            CustomerId = customerId;
            FirstName = fName;
            LastName = lName;
            DateIssued = issued;
            DateReturned = returned;
        }
    }
}
