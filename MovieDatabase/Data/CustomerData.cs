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

        public ObservableCollection<CustomerModel> CustomerList { get; set; }

        public CustomerModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCustomer)));
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

        public CustomerData()
        {
            CustomerList = new ObservableCollection<CustomerModel>();

            DatabaseOperations database = new DatabaseOperations();
            _allCustomers = database.GetAllCustomerDataToList().Result;

            PerformFiltering();
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
