using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> GetAllAsync();
        Task<UserRole> GetByIdAsync(string id);
        Task CreateAsync(UserRole role);
        Task UpdateAsync(string id, UserRole role);
        Task DeleteAsync(string id);
    }
}
