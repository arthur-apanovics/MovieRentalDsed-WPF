using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MovieDatabase.Models;

namespace MovieDatabase
{
    public class DatabaseOperations
    {
        /// <summary>
        /// Initializing variables.
        /// </summary>

        public static event EventHandler DataUpdateComplete;

        readonly static string _connectionString = DatabaseConnectionString.String;
        SqlConnection _connection = new SqlConnection(_connectionString);



        /// <summary>
        /// Some maintenance methods
        /// </summary>

        private void DataUpdateEventAndSignature(string methodName)
        {
            Console.WriteLine($"Database updated on {DateTime.Now} from {this.ToString()}, \n method called: {methodName}");
            DataUpdateComplete(this, EventArgs.Empty);
        }

        private static void PrintException(Exception e)
        {
            MessageBox.Show(e.ToString());
            Console.WriteLine(e);
        }

        private static void CatchSqlConnectionExceptions(SqlException sqlException)
        {
            var retry = MessageBox.Show($"An error occured during connection:\n{sqlException.Message}\n\nUsually a retry fixes the error.\nTry again?", "SQL Connection Error", MessageBoxButtons.RetryCancel);
            // TODO Restart creates a SECOND instance of the application ?!
            if (retry == DialogResult.Retry)
            {
                Application.Restart();
            }
            // TODO Exit DOES NOT close the application ?!
            else
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// ADO.Net methods
        /// </summary>

        //Customer data
        public async Task<List<CustomerModel>> GetAllCustomerDataToList()
        {
            List<CustomerModel> customersIn = new List<CustomerModel>();

            using (_connection)
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllCustomers";

                try
                {
                    await _connection.OpenAsync().ConfigureAwait(false);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        customersIn.Add(new CustomerModel(
                            Convert.ToInt32(reader["CustID"]),
                            reader["FirstName"].ToString(),
                            reader["LastName"].ToString(),
                            reader["Address"].ToString(),
                            reader["Phone"].ToString()));
                    }
                    reader.Close();
                }
                catch (SqlException sqlException)
                {
                    CatchSqlConnectionExceptions(sqlException);
                }
                catch (Exception e)
                {
                    PrintException(e);
                }
                finally
                {
                    _connection.Close();
                }

                return customersIn;
            }
        }

        // Movie data
        public async Task<List<MovieModel>> GetAllMovieDataToList()
        {
            List<MovieModel> moviesIn = new List<MovieModel>();

            using (_connection)
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllMovies";

                try
                {
                    await _connection.OpenAsync().ConfigureAwait(false);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        moviesIn.Add(new MovieModel(
                            Convert.ToInt32(reader["MovieID"]),
                            reader["Rating"].ToString(),
                            reader["Title"].ToString(),
                            reader["Year"].ToString(),
                            Convert.ToInt32(reader["Rental_Cost"]),
                            Convert.ToInt32(reader["Copies"]),
                            reader["Plot"].ToString(),
                            reader["Genre"].ToString()));
                    }
                    reader.Close();
                }
                catch (SqlException sqlException)
                {
                    CatchSqlConnectionExceptions(sqlException);
                }
                catch (Exception e)
                {
                    PrintException(e);
                }
                finally
                {
                    _connection.Close();
                }

                return moviesIn;
            }
        }

        //Rentals data
        public async Task<List<RentedMovieModel>> GetAllRentedMovies()
        {
            List<RentedMovieModel> rentedIn = new List<RentedMovieModel>();

            using (_connection)
            {
                String query = "SELECT * FROM dbo.CurrentlyRentedMovies ORDER by DateRented desc";
                SqlCommand cmd = new SqlCommand(query, _connection) { CommandType = CommandType.Text };
                try
                {
                    await _connection.OpenAsync().ConfigureAwait(false);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        rentedIn.Add(new RentedMovieModel(
                            Convert.ToInt32(reader["MovieID"]),
                            Convert.ToInt32(reader["CustIDFK"]),
                            reader["FirstName"].ToString(),
                            reader["LastName"].ToString(),
                            reader["Title"].ToString(),
                            Convert.ToDateTime(reader["DateRented"]),
                            String.IsNullOrEmpty(reader["DateReturned"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["DateReturned"])));
                    }
                    reader.Close();
                }
                catch (SqlException sqlException)
                {
                    CatchSqlConnectionExceptions(sqlException);
                }
                catch (Exception e)
                {
                    PrintException(e);
                }
                finally
                {
                    _connection.Close();
                }

                return rentedIn;
            }
        }

        // Update movie data
        public void UpdateMovieInTable(MovieModel movie)
        {
            string updateString = "update dbo.Movies set " +
                                   "Rating = @rating, Title = @title, Year = @year, Rental_Cost = @price, Copies = @copies, Plot = @plot, Genre = @genre " +
                                   "where MovieID = @id";

            SqlCommand updateCommand = new SqlCommand(updateString, _connection);
            updateCommand.Parameters.AddWithValue("@id", movie.MovieId);
            updateCommand.Parameters.AddWithValue("@rating", movie.Rating);
            updateCommand.Parameters.AddWithValue("@title", movie.Title);
            updateCommand.Parameters.AddWithValue("@year", movie.Year);
            updateCommand.Parameters.AddWithValue("@price", movie.RentalPrice);
            updateCommand.Parameters.AddWithValue("@copies", movie.Copies);
            updateCommand.Parameters.AddWithValue("@plot", movie.Plot);
            updateCommand.Parameters.AddWithValue("@genre", movie.Genre);

            _connection.Open();
            updateCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(UpdateMovieInTable));
        }

        // Add new movie to database
        public void AddMovieToTable(MovieModel movie)
        {
            string insertString = "insert into dbo.Movies " +
                                  "(Rating, Title, Year, Rental_Cost, Copies, Plot, Genre)" +
                                  "values " +
                                  "(@rating, @title, @year, @price, @copies, @plot, @genre)";

            SqlCommand insertCommand = new SqlCommand(insertString, _connection);
            insertCommand.Parameters.AddWithValue("@rating", movie.Rating);
            insertCommand.Parameters.AddWithValue("@title", movie.Title);
            insertCommand.Parameters.AddWithValue("@year", movie.Year);
            insertCommand.Parameters.AddWithValue("@price", movie.RentalPrice);
            insertCommand.Parameters.AddWithValue("@copies", movie.Copies);
            insertCommand.Parameters.AddWithValue("@plot", movie.Plot);
            insertCommand.Parameters.AddWithValue("@genre", movie.Genre);

            _connection.Open();
            insertCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(AddMovieToTable));
        }

        // Delete movie from table
        public void DeleteMovieFromTable(MovieModel movie)
        {
            string deleteString = "delete from dbo.Movies where MovieID = @MovieID";

            SqlCommand delCommand = new SqlCommand(deleteString, _connection);
            delCommand.Parameters.AddWithValue("@MovieID", movie.MovieId);

            _connection.Open();
            delCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(DeleteMovieFromTable));
        }

        // Add new rented movie
        public void RentOutMovie(MovieModel movie, CustomerModel customer)
        {
            string insertString = "insert into dbo.RentedMovies " +
                                  "(MovieIDFK, CustIDFK, DateRented)" +
                                  "values " +
                                  "(@MovieIDFK, @CustIDFK, @DateRented)";

            SqlCommand insertCommand = new SqlCommand(insertString, _connection);
            insertCommand.Parameters.AddWithValue("@MovieIDFK", movie.MovieId);
            insertCommand.Parameters.AddWithValue("@CustIDFK", customer.CustId);
            insertCommand.Parameters.AddWithValue("@DateRented", DateTime.Now);

            _connection.Open();
            insertCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(RentOutMovie));
            //Done. implement OnDataUpdateComplete to give more information about what updates have been made.
            Console.WriteLine("Movie rented");
        }
        
        // Update customer data
        public void UpdateCustomerInTable(CustomerModel customer)
        {
            string updateString = "update dbo.Customer set " +
                                  "FirstName = @FirstName, LastName = @LastName, Address = @Address, Phone = @Phone " +
                                  "where CustID = @CustID";

            SqlCommand insertCommand = new SqlCommand(updateString, _connection);
            insertCommand.Parameters.AddWithValue("@CustID", customer.CustId);
            insertCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
            insertCommand.Parameters.AddWithValue("@LastName", customer.LastName);
            insertCommand.Parameters.AddWithValue("@Address", customer.Address);
            insertCommand.Parameters.AddWithValue("@Phone", customer.Phone);

            _connection.Open();
            insertCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(UpdateCustomerInTable));
        }

        // Add new customer to database
        public void AddCustomerToTable(CustomerModel customer)
        {
            string insertString = "insert into dbo.Customer " +
                                  "(FirstName, LastName, Address, Phone)" +
                                  "values " +
                                  "(@FirstName, @LastName, @Address, @Phone)";

            SqlCommand insertCommand = new SqlCommand(insertString, _connection);
            insertCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
            insertCommand.Parameters.AddWithValue("@LastName", customer.LastName);
            insertCommand.Parameters.AddWithValue("@Address", customer.Address);
            insertCommand.Parameters.AddWithValue("@Phone", customer.Phone);

            _connection.Open();
            insertCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(AddCustomerToTable));
        }

        public void DeleteCustomerFromTable(CustomerModel cust)
        {
            string deleteString = "delete from dbo.Customer where CustId = @CustId";

            SqlCommand delCommand = new SqlCommand(deleteString, _connection);
            delCommand.Parameters.AddWithValue("@CustId", cust.CustId);

            _connection.Open();
            delCommand.ExecuteNonQuery();
            _connection.Close();

            DataUpdateEventAndSignature(nameof(DeleteCustomerFromTable));
        }
    }
}