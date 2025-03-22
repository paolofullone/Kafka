using System.Data;

namespace Infrastructure.DbFactory.Interfaces
{
    public interface ISQLConnectionFactory
    {
        string GetConnectionString();
        IDbConnection GetConnection();
    }
}
