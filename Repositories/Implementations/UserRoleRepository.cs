using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.MongoDB;
using best_hackathon_2025.Repositories.Interfaces;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IMongoCollection<UserRole> _roles;

        public UserRoleRepository(MongoDbContext context)
        {
            _roles = context.Database.GetCollection<UserRole>("UserRoles");
        }

        public async Task<List<UserRole>> GetAllAsync() => await _roles.Find(_ => true).ToListAsync();
        public async Task<UserRole> GetByIdAsync(string id) => await _roles.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(UserRole role) => await _roles.InsertOneAsync(role);
        public async Task UpdateAsync(string id, UserRole role) => await _roles.ReplaceOneAsync(x => x.Id == id, role);
        public async Task DeleteAsync(string id) => await _roles.DeleteOneAsync(x => x.Id == id);
    }
}
