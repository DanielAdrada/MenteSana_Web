using MySql.Data.MySqlClient;
using System.Configuration;

namespace Data
{
    public class Persistence
    {
        private string _connectionString;

        public Persistence()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["Conn"]
                .ConnectionString;
        }

        public MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
