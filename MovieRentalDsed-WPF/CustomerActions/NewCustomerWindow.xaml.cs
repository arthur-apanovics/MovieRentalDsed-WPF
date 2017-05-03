using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MovieDatabase;
using MovieDatabase.Models;

namespace MovieRentalDsed_WPF.CustomerActions
{
    /// <summary>
    /// Interaction logic for NewCustomerWindow.xaml
    /// </summary>
    public partial class NewCustomerWindow : Window
    {
        public NewCustomerWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new DatabaseOperations().AddCustomerToTable(new CustomerModel(
                    Convert.ToInt32(txtId.Text),
                    txtFName.Text,
                    txtLName.Text,
                    txtAddress.Text,
                    txtPhone.Text
                ));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                MessageBox.Show(exception.Message);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
