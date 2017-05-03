using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class MovieData : INotifyPropertyChanged
    {
        private List<MovieModel> _allMovieData;
        private MovieModel _selectedMovie;
        private string _filter;
        private List<RentedMovieModel> _selectedMovieRentalStatus;

        public MovieData()
        {
            MovieList = new ObservableCollection<MovieModel>();

            DatabaseOperations database = new DatabaseOperations();
            _allMovieData = database.GetAllMovieDataToList().Result;

            RentedMovies = new RentedMovieData().RentedMovies;

            FilterUnreturnedMovies(RentedMovies);

            PerformFiltering();
        }

        /// <summary>
        /// Properties 
        /// </summary>

        public ObservableCollection<MovieModel> MovieList { get; set; }

        public List<RentedMovieModel> RentedMovies { get; set; }

        public List<RentedMovieModel> UnreturnedMovies { get; set; }

        public List<RentedMovieModel> SelectedMovieRentalHistory
        {
            get { return _selectedMovieRentalStatus; }
            set
            {
                _selectedMovieRentalStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMovieRentalHistory)));
            }
        }

        public MovieModel SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                SelectedMovieRentalHistory = value != null ? RentedMovies.FindAll(x => x.MovieId.Equals(value.MovieId)) : null;

                _selectedMovie = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMovie)));
            }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter)
                    return;
                _filter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
                PerformFiltering();
            }
        }

        /// <summary>
        /// Methods
        /// </summary>

        private void PerformFiltering()
        {
            if (_filter == null)
                _filter = string.Empty;

            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            var result =
                _allMovieData.Where(d => d.Title.ToLowerInvariant().Contains(lowerCaseFilter)).ToList();

            var toRemove = MovieList.Except(result).ToList();

            foreach (var x in toRemove)
                MovieList.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > MovieList.Count || !MovieList[i].Equals(resultItem))
                    MovieList.Insert(i, resultItem);
            }
        }

        private void FilterUnreturnedMovies(List<RentedMovieModel> rentedMovies)
        {
            UnreturnedMovies = rentedMovies.Where(x => x.DateReturned.Equals(DateTime.MinValue)).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
