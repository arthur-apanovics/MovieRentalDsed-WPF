using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class CustomerData
    {
        //TODO Merge DATA into a single class to eliminate the need to call the database every time.
        public List<CustomerModel> CustomerList { get; set; }

        DatabaseOperations database = new DatabaseOperations();

        public CustomerData()
        {
            RefreshCustomers();
        }

        public void RefreshCustomers()
        {
            CustomerList = database.GetAllCustomerDataToList().Result;
        }
    }
}
