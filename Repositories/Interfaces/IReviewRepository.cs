using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review> GetByIdAsync(string id);
        Task CreateAsync(Review review);
        Task UpdateAsync(string id, Review review);
        Task DeleteAsync(string id);
    }
}
