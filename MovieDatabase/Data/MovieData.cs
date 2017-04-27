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
        private MovieModel _selectedMovie;
        private string _filter = null;

        public MovieData()
        {
            DatabaseOperations database = new DatabaseOperations();

            MovieList = database.GetAllMovieDataToList().Result;
        }

        /// <summary>
        /// Properties 
        /// </summary>

        public ObservableCollection<MovieModel> MovieList { get; set; }

        public MovieModel SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
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
                MovieList.Where(d => d.Title.ToLowerInvariant().Contains(lowerCaseFilter)).ToList();

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

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
