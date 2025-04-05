using Infrastructure.DbFactory.Interfaces;
using System.Data;

namespace Infrastructure.DbFactory
{
    public class OracleConnectionFactory(string connectionString) : IOracleConnectionFactory
    {
        public string GetConnectionString()
        {
            return connectionString;
        }

        public IDbConnection GetConnection()
        {
            // using dapper

            var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
            connection.Open();
            return connection;

        }


    }
}
