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
using MovieDatabase.Models;

namespace MovieRentalDsed_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //TODO Add a dialog box for database server credentials.
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllData(this, EventArgs.Empty);
        }

        private void btnIssueMovie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMovieOperation_Click(object sender, RoutedEventArgs e)
        {
            // create an event listener to refresh info on database updates
            DatabaseOperations.DataUpdateComplete += UpdateAllData;

            var btnType = sender as Button;
            int currentSelectedItem = MovieNames.SelectedIndex;

            //todo_done cleanup repeated code.
            if (btnType.Name == "btnEditMovie")
            {
                var editDialog = new EditMovieWindow(MovieNames.SelectedItem as MovieModel);
                editDialog.ShowDialog();
            }
            else
            {
                var addDialog = new AddNewMovieWindow();
                addDialog.ShowDialog();
            }

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

        private void btnClearText_Click(object sender, RoutedEventArgs e)
        {
            txtSearchMovies.Text = String.Empty;
            txtSearchCustomers.Text = string.Empty;
        }
    }
}