using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase;
using MovieDatabase.Models;

namespace MovieRentalDsed_WPF
{
    class RentalIssuer
    {

        public RentalIssuer(MovieModel movie, CustomerModel customer)
        {
            new DatabaseOperations().RentOutMovie(movie, customer);
        }

    }
}
