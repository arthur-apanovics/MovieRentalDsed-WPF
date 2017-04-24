using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.Models
{
    public class CustomerModel
    {
        public int CustId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public CustomerModel(int id, string fName, string lName, string addr, string phone)
        {
            CustId = id;
            FirstName = fName;
            LastName = lName;
            Address = addr;
            Phone = phone;
        }
    }
}
