using Npgsql;

namespace Restaurant.Persistence.Connectors
{
    public class ConnectionManagerBase
    {
        private static string ConnectionString { get; set; }

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
