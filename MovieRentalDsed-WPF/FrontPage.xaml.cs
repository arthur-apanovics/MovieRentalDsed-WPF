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
using MovieRentalDsed_WPF.CustomerActions;

namespace MovieRentalDsed_WPF
{
    public partial class MainWindow : Window
    {
        private MovieModel _movieToIssue;
        private CustomerModel _customerToIssue;

        public MainWindow()
        {
            InitializeComponent();
            //TODO Add a dialog box for database server credentials.

            //var movieDataCtor = new MovieData();
        }

        /// <summary>
        /// Click events here
        /// </summary>


        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllData(this, EventArgs.Empty);
        }

        private void btnIssueMovie_Click(object sender, RoutedEventArgs e)
        {
            MovieIssuing(sender);
        }

        private void btnReturnMovie_Click(object sender, RoutedEventArgs e)
        {
            ReturnMovieHandler(sender);
        }

        private void DataOperation_Click(object sender, RoutedEventArgs e)
        {
            DataOperation(sender);
        }

        private void CustomerRentals_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = sender as ListBox;
            NavigateToMovie(selected.SelectedItem);
        }

        private void btnNav_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var rentedItem = RentedMovies.SelectedItem as RentedMovieModel;

            if (btn.Name == "btnNavToMovie")
            {
                NavigateToMovie(rentedItem);
            }
            else if (btn.Name == "btnNavToCust")
            {
                NavigateToCustomer(rentedItem);
            }
        }

        private void rented_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = sender as ListBox;
            NavigateToCustomer(selected.SelectedItem);
        }

        private void btnClearText_Click(object sender, RoutedEventArgs e)
        {
            txtSearchMovies.Text = String.Empty;
            txtSearchCustomers.Text = string.Empty;
        }

        private void btnSelectedItemCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetToNoSelection();
        }

        /// <summary>
        /// Methods here
        /// </summary>

        private void MovieIssuing(object sender)
        {
            var btn = sender as Button;
            if (btn.Name == "btnIssueMovie")
            {
                _movieToIssue = MovieNames.SelectedItem as MovieModel;

                if (_customerToIssue == null)
                {
                    lblSelectedItem.Text = $"Select customer to rent \"{_movieToIssue.Title}\" or";
                    //todo clean up tabindexes for controls on form. 
                    tiCustomers.IsSelected = true;
                    //todo add animation to txtSearchCustomers to focus user attention
                    txtSearchCustomers.Focus(); //does not focus for some reason
                }
            }
            else if (btn.Name == "btnIssueMovieToCust")
            {
                _customerToIssue = CustomerNames.SelectedItem as CustomerModel;

                if (_movieToIssue == null)
                {
                    lblSelectedItem.Text = $"Select a movie to rent to {_customerToIssue.FullName}";
                    tiMovies.IsSelected = true;
                    //todo add animation to txtSearchMovies to focus user attention
                    txtSearchMovies.Focus(); //does not focus for some reason
                }
            }

            if (_movieToIssue != null && _customerToIssue != null)
            {
                DatabaseOperations.DataUpdateComplete += UpdateAllData;

                new RentalIssuer(_movieToIssue, _customerToIssue);

                DatabaseOperations.DataUpdateComplete -= UpdateAllData;

                MessageBox.Show("Movie has been issued!");

                //reset variables back to null after movie has been issued.
                ResetToNoSelection();
            }
        }

        private void ReturnMovieHandler(object sender)
        {
            var btn = sender as Button;

            if (btn.Name == "btnReturnAMovie")
            {
                tiUnreturned.IsSelected = true;
            }
            else
            {
                ReturnMovie(btn.Name);
            }
        }

        private void ReturnMovie(string senderName)
        {
            // create an event listener to refresh info on database updates
            DatabaseOperations.DataUpdateComplete += UpdateAllData;

            // Dummy variable to stop intellisense from complaining about possible null reference
            var rentedItem = new RentedMovieModel(0, 0, "dummy", "name", "", DateTime.MinValue, DateTime.MinValue);
            if (senderName == "btnReturnMovieViaCust")
            {
                rentedItem = CustRentCurrent.SelectedItem as RentedMovieModel;
            }
            else if (senderName == "btnReturnMovieViaRented")
            {
                rentedItem = RentedMovies.SelectedItem as RentedMovieModel;
            }
            
            var choice = MessageBox.Show(
                $"Return \"{rentedItem.MovieTitle}\"?\n\n" +
                $"Rented by {rentedItem.FirstName} {rentedItem.LastName}\n" +
                $"Rented on {rentedItem.DateIssued}",
                "Confirm Return",
                MessageBoxButton.OKCancel);
            if (choice == MessageBoxResult.OK)
            {
                new DatabaseOperations().ReturnMovie(rentedItem);
            }

            // since the event is static (to make it global), detach listener to avoid duplicate event triggers
            DatabaseOperations.DataUpdateComplete -= UpdateAllData;
        }

        private void DataOperation(object sender)
        {
            // create an event listener to refresh info on database updates
            DatabaseOperations.DataUpdateComplete += UpdateAllData;

            var btn = sender as Button;

            //todo_done cleanup repeated code.
            if (btn.Name == "btnEditMovie")
            {
                var editDialog = new EditMovieWindow(MovieNames.SelectedItem as MovieModel);
                editDialog.ShowDialog();
            }
            else if (btn.Name == "btnNewMovie")
            {
                var addDialog = new AddNewMovieWindow();
                addDialog.ShowDialog();
            }
            else if (btn.Name == "btnEditCust")
            {
                var editCustDialog = new EditCustomerWindow(CustomerNames.SelectedItem as CustomerModel);
                editCustDialog.ShowDialog();
            }
            else if (btn.Name == "btnNewCustomer")
            {
                var addDialog = new NewCustomerWindow();
                addDialog.ShowDialog();
            }
            else if (btn.Name == "btnDeleteMovie")
            {
                var movie = MovieNames.SelectedItem as MovieModel;
                var choice = MessageBox.Show($"Are you sure you want to delete \"{movie.Title}\"?", "Confirm Delete", MessageBoxButton.OKCancel);
                if (choice == MessageBoxResult.OK)
                {
                    new DatabaseOperations().DeleteMovieFromTable(movie);
                }
            }
            else if (btn.Name == "btnDeleteCust")
            {
                var cust = CustomerNames.SelectedItem as CustomerModel;
                var choice = MessageBox.Show($"Are you sure you want to delete \"{cust.FullName}\"?", "Confirm Delete", MessageBoxButton.OKCancel);
                if (choice == MessageBoxResult.OK)
                {
                    new DatabaseOperations().DeleteCustomerFromTable(cust);
                }
            }

            // since the event is static (to make it global), detach listener to avoid duplicate event triggers
            DatabaseOperations.DataUpdateComplete -= UpdateAllData;
        }

        private void UpdateAllData(object sender, EventArgs e)
        {
            int prevSelMovie = MovieNames.SelectedIndex;
            int prevSelCustomer = CustomerNames.SelectedIndex;

            tiMovies.DataContext = new MovieData();
            tiCustomers.DataContext = new CustomerData();
            tiUnreturned.DataContext = new RentedMovieData();

            ReselectItemsAfterDataUpdate(prevSelMovie, prevSelCustomer);

            Console.WriteLine($"Data in window has been refreshed, from {this.ToString()}");
        }

        private void ResetToNoSelection()
        {
            _customerToIssue = null;
            _movieToIssue = null;
            lblSelectedItem.Text = string.Empty;
        }

        private void ReselectItemsAfterDataUpdate(int prevSelMovie, int prevSelCustomer)
        {
            MovieNames.SelectedIndex = prevSelMovie;
            CustomerNames.SelectedIndex = prevSelCustomer;
        }

        private void NavigateToCustomer(object sender)
        {
            var selection = sender as RentedMovieModel;

            CustomerNames.SelectedIndex = selection.CustomerId - 1;
            CustomerNames.ScrollIntoView(CustomerNames.SelectedItem);

            tiCustomers.Focus();
        }

        private void NavigateToMovie(object sender)
        {
            var selection = sender as RentedMovieModel;

            MovieNames.SelectedIndex = selection.MovieId - 1;
            MovieNames.ScrollIntoView(MovieNames.SelectedItem);

            tiMovies.Focus();
        }
    }
}