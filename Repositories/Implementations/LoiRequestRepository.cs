using best_hackathon_2025.MongoDB;
using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class LoiRequestRepository : ILoiRequestRepository
    {
        private readonly IMongoCollection<LoiRequest> _collection;

        public LoiRequestRepository(MongoDbContext context)
        {
            _collection = context.LoiRequest;
        }

        public async Task<List<LoiRequest>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<LoiRequest> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(LoiRequest loiRequest)
        {
            await _collection.InsertOneAsync(loiRequest);
        }

        public async Task UpdateAsync(string id, LoiRequest loiRequest)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, loiRequest);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
