using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface ITransportRepository
    {
        Task<List<Transport>> GetAllAsync();
        Task<Transport> GetByIdAsync(string id);
        Task CreateAsync(Transport transport);
        Task UpdateAsync(string id, Transport transport);
        Task DeleteAsync(string id);
    }

}
