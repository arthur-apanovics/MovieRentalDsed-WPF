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

namespace MovieRentalDsed_WPF
{
    /// <summary>
    /// Interaction logic for AddNewMovieWindow.xaml
    /// </summary>
    public partial class AddNewMovieWindow : Window
    {
        public AddNewMovieWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            InsertSender();
        }

        private void InsertSender()
        {
            MovieModel movie = null;

            // Too lazy to cath user input exceprions. Catch all instead.
            try
            {
                movie = new MovieModel(
                    -1,
                    txtRating.Text,
                    txtTitle.Text,
                    txtYear.Text,
                    Convert.ToInt32(txtPrice.Text),
                    Convert.ToInt32(txtCopies.Text),
                    txtPlot.Text,
                    txtGenre.Text);

                var insert = new DatabaseOperations();
                insert.AddMovieToTable(movie);

                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

                Console.WriteLine($"ERROR UPDATING DATABASE:\n{exception}");
                return;
            }
        }
    }
}