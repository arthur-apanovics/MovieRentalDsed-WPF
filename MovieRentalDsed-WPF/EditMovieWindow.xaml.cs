﻿using System;
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
using MovieDatabase.Data;
using MovieDatabase.Models;
using MovieDatabase;

namespace MovieRentalDsed_WPF
{
    /// <summary>
    /// Interaction logic for EditMovieWindow.xaml
    /// </summary>
    public partial class EditMovieWindow : Window
    {
        public EditMovieWindow(MovieModel movie)
        {
            InitializeComponent();
            PopulateFields(movie);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            UpdateSender();
        }

        /// <section>
        /// Constructor & click methods here
        /// </section>
        

        private void PopulateFields(MovieModel movie)
        {
            txtTitle.Text = NullChecker(movie.Title);
            txtCopies.Text = NullChecker(movie.Copies.ToString());
            txtGenre.Text = NullChecker(movie.Genre);
            txtId.Text = NullChecker(movie.MovieId.ToString());
            txtPlot.Text = NullChecker(movie.Plot);
            txtPrice.Text = NullChecker(movie.RentalPrice.ToString());
            txtRating.Text = NullChecker(movie.Rating);
            txtYear.Text = NullChecker(movie.Year);
        }

        private void UpdateSender()
        {
            MovieModel movie = null;
            try
            {
                movie = new MovieModel(
                    Convert.ToInt32(txtId.Text),
                    txtRating.Text,
                    txtTitle.Text,
                    txtYear.Text,
                    Convert.ToInt32(txtPrice.Text),
                    Convert.ToInt32(txtCopies.Text),
                    txtPlot.Text,
                    txtGenre.Text
                );

                var update = new UpdateTables();
                update.UpdateMovie(movie);

                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return;
            }
        }

        private string NullChecker(string input)
        {
            string output;

            output = String.IsNullOrEmpty(input) ? "%EMPTY%" : input;

            return output;
        }

    }
}
