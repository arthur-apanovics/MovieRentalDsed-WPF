using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieDatabase;
using MovieDatabase.Data;
using MovieDatabase.Delegates;
using MovieDatabase.Models;

namespace MovieRentalDsed_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllData(this, EventArgs.Empty);
        }

        private void btnIssueMovie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditMovie_Click(object sender, RoutedEventArgs e)
        {
            // create an event listener to refresh info on database updates
            DatabaseOperations.DataUpdateComplete += UpdateAllData;

            int currentSelectedItem = MovieNames.SelectedIndex;

            var editDialog = new EditMovieWindow(MovieNames.SelectedItem as MovieModel);
            editDialog.ShowDialog();

            // on data update, selected item gets reset to -1. This method sets the value back to what it was before the update.
            ReselectMovieAfterDataUpdate(currentSelectedItem);

            // since the event is static (to make it global), detach listener to avoid duplicate event triggers
            DatabaseOperations.DataUpdateComplete -= UpdateAllData;
        }

        private void btnNewMovie_Click(object sender, RoutedEventArgs e)
        {
            //todo cleanup repeated code.
            // create an event listener to refresh info on database updates
            DatabaseOperations.DataUpdateComplete += UpdateAllData;

            int currentSelectedItem = MovieNames.SelectedIndex;

            var addDialog = new AddNewMovieWindow();
            addDialog.ShowDialog();

            // on data update, selected item gets reset to -1. This method sets the value back to what it was before the update.
            ReselectMovieAfterDataUpdate(currentSelectedItem);

            // since the event is static (to make it global), detach listener to avoid duplicate event triggers
            DatabaseOperations.DataUpdateComplete -= UpdateAllData;
        }

        private void UpdateAllData(object sender, EventArgs e)
        {
            tiMovies.DataContext = new MovieData();
            tiCustomers.DataContext = new CustomerData();

            Console.WriteLine($"Data in window has been refreshed, from {this.ToString()}");
        }

        private void ReselectMovieAfterDataUpdate(int previousSelection)
        {
            MovieNames.SelectedIndex = previousSelection;
        }
    }
}
