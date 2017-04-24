using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Models
{
    public class MovieModel
    {
        public int MovieId { get; set; }
        public string Rating { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public int RentalPrice { get; set; }
        public int Copies { get; set; }
        public string Plot { get; set; }
        public string Genre { get; set; }

        public MovieModel(int id, string rating, string title, string year, int price, int copies, string plot, string genre )
        {
            MovieId = id;
            Rating = rating;
            Title = title;
            Year = year;
            RentalPrice = price;
            Copies = copies;
            Plot = plot;
            Genre = genre;
        }
    }
}
