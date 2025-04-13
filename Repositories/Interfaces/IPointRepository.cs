using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface IPointRepository
    {
        Task<List<Point>> GetAllAsync();
        Task<Point> GetByIdAsync(string id);
        Task CreateAsync(Point point);
        Task UpdateAsync(string id, Point point);
        Task DeleteAsync(string id);
    }

}
