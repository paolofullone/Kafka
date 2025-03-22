using System.Data;

namespace Infrastructure.DbFactory.Interfaces
{
    interface IOracleDb
    {
        string GetConnectionString();
        IDbConnection GetConnection();
    }
}
