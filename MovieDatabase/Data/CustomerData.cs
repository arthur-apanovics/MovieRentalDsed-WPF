using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class CustomerData : INotifyPropertyChanged
    {
        private CustomerModel _selectedCustomer;
        private string _filter;
        private List<CustomerModel> _allCustomers;
        private List<RentedMovieModel> _allRentedMovies;

        public ObservableCollection<CustomerModel> CustomerList { get; set; }
        public List<RentedMovieModel> CustomerRentalHistory { get; set; }
        public List<RentedMovieModel> CustomerCurrentRentals { get; set; }

        public CustomerModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCustomer)));

                // find rentals for selected customer and update properties.
                if (value != null)
                {
                    FindCustomerRentedMovies(value, _allRentedMovies);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomerRentalHistory)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomerCurrentRentals)));
                }
            }
        }

        private void FindCustomerRentedMovies(CustomerModel selectedCustomer, List<RentedMovieModel> allRentals)
        {
            CustomerRentalHistory = new List<RentedMovieModel>();
            CustomerCurrentRentals = new List<RentedMovieModel>();

            for (int i = 0; i < allRentals.Count; i++)
            {
                if (selectedCustomer.CustId == allRentals[i].CustomerId)
                {
                    // if rental has been returned, add to past rental list
                    if (allRentals[i].DateReturned != DateTime.MinValue)
                    {
                        CustomerRentalHistory.Add(allRentals[i]);
                    }
                    // else add to current, unreturned rental list
                    else
                    {
                        CustomerCurrentRentals.Add(allRentals[i]);
                    }
                }
            }
        }

        // CTOR here
        public CustomerData()
        {
            CustomerList = new ObservableCollection<CustomerModel>();


            DatabaseOperations database = new DatabaseOperations();
            _allCustomers = database.GetAllCustomerDataToList().Result;
            _allRentedMovies = new RentedMovieData().RentedMovies;

            PerformFiltering();
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
                _allCustomers.Where(d => d.FullName.ToLowerInvariant().Contains(lowerCaseFilter)).ToList();

            var toRemove = CustomerList.Except(result).ToList();

            foreach (var x in toRemove)
                CustomerList.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > CustomerList.Count || !CustomerList[i].Equals(resultItem))
                    CustomerList.Insert(i, resultItem);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
