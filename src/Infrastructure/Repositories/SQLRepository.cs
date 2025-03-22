using Dapper;
using Domain.Models;
using Infrastructure.DbFactory.Interfaces;
using Infrastructure.Repositories.Queries;

namespace Infrastructure.Repositories
{
    public class SQLRepository(ISQLConnectionFactory connectionFactory) : ISQLRepository
    {

        public async Task<List<SampleMessage>> GetAll(CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetConnection();

            var messages = (await connection.QueryAsync<SampleMessage>(SampleMessageQueries.GetAll)).ToList();

            return messages;
        }

        public async Task<int> AddAsync(SampleMessage message, CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.GetConnection();

            var insert = await connection.ExecuteAsync(SampleMessageQueries.Add, message);

            return insert;

        }
    }
}
