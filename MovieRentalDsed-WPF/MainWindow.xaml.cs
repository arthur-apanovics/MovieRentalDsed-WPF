using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            tiMovies.DataContext = new MovieData();
            tiCustomers.DataContext = new CustomerData();
        }

        private void btnIssueMovie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditMovie_Click(object sender, RoutedEventArgs e)
        {
            var editDialog = new EditMovieWindow(MovieNames.SelectedItem as MovieModel);
            editDialog.ShowDialog();
        }
    }
}
