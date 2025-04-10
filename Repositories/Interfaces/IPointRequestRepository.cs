using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface IPointRequestRepository
    {
        Task<List<PointRequest>> GetAllAsync();
        Task<PointRequest> GetByIdAsync(string id);
        Task CreateAsync(PointRequest pointRequest);
        Task UpdateAsync(string id, PointRequest pointRequest);
        Task DeleteAsync(string id);
    }
}
