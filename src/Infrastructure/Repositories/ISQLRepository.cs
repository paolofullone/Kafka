using Domain.Models;

namespace Infrastructure.Repositories
{
    public interface ISQLRepository
    {
        Task<List<SampleMessage>> GetAll(CancellationToken cancellationToken); 
        Task<int> AddAsync(SampleMessage message, CancellationToken cancellationToken);
    }
}