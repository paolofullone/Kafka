using Infrastructure.DbFactory.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.DbFactory
{
    public class SQLConnectionFactory(string connectionString) : ISQLConnectionFactory
    {
        public string GetConnectionString()
        {
            return connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
