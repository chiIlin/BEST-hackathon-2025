﻿using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<List<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(string id, User user);
        Task DeleteAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task SavePointAsync(string userId, string pointId);
        Task<List<User>> GetManyByIdsAsync(List<string> ids);
        Task RemovePointAsync(string userId, string pointId);
        Task RemoveSavedPointAsync(string userId, string pointId);

    }

}
