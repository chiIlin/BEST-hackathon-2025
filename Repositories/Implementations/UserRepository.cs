using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.MongoDB;
using MongoDB.Driver;
using best_hackathon_2025.Repositories.Interfaces;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<User> GetByIdAsync(string id)
            => await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<User>> GetAllAsync()
            => await _users.Find(_ => true).ToListAsync();

        public async Task CreateAsync(User user)
            => await _users.InsertOneAsync(user);

        public async Task UpdateAsync(string id, User user)
            => await _users.ReplaceOneAsync(x => x.Id == id, user);

        public async Task DeleteAsync(string id)
            => await _users.DeleteOneAsync(x => x.Id == id);
        public async Task<User> GetByEmailAsync(string email) 
            => await _users.Find(Builders<User>.Filter.Eq("Email", email)).FirstOrDefaultAsync();

        public async Task SavePointAsync(string userId, string pointId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.AddToSet(x => x.Points, pointId);
            await _users.UpdateOneAsync(filter, update);
        }

    }

}
