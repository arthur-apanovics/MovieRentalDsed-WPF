using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Annotations;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class MovieData : INotifyPropertyChanged
    {
        private ObservableCollection<MovieModel> _movieList;
        private MovieModel _selectedMovie;

        public ObservableCollection<MovieModel> MovieList
        {
            get { return _movieList; }
            set
            {
                _movieList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MovieList)));
            }
        }

        public MovieModel SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                _selectedMovie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMovie)));
            }
        }

        public MovieData()
        {
            DatabaseOperations database = new DatabaseOperations();

            MovieList = database.GetAllMovieDataToList().Result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
