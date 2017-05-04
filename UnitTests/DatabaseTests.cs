using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void TestConnection()
        {
            string connectionString = MovieDatabase.DatabaseConnectionString.String;

            string queryString = "SELECT COUNT(RMID) FROM dbo.RentedMovies";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand Command = new SqlCommand(queryString, connection);
                connection.Open();
                var result = Convert.ToInt32(Command.ExecuteScalar());
                connection.Close();

                Assert.IsNotNull(result);
            }
        }
    }
}
