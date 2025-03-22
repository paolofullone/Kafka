using MongoDB.Driver;

namespace Infrastructure.DbFactory.Interfaces
{
    interface IMongoDb
    {
        string GetConnectionString();
        IMongoDatabase CreateConnection();
    }
}
