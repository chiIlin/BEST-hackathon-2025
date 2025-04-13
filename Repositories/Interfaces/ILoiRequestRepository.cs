using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface ILoiRequestRepository
    {
        Task<List<LoiRequest>> GetAllAsync();
        Task<LoiRequest> GetByIdAsync(string id);
        Task CreateAsync(LoiRequest loiRequest);
        Task UpdateAsync(string id, LoiRequest loiRequest);
        Task DeleteAsync(string id);
        Task UpdatePointLoiAsync(string pointId, int newLoi);

        Task UpdatePointManualLoiAsync(string pointId, int newManualLoi);

    }
}
