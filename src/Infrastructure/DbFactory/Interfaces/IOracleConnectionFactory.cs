using System.Data;

namespace Infrastructure.DbFactory.Interfaces
{
    public interface IOracleConnectionFactory
    {
        string GetConnectionString();
        IDbConnection GetConnection();
    }
}
