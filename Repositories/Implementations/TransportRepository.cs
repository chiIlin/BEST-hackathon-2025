using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.MongoDB;
using best_hackathon_2025.Repositories.Interfaces;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class TransportRepository : ITransportRepository
    {
        private readonly IMongoCollection<Transport> _transport;

        public TransportRepository(MongoDbContext context)
        {
            _transport = context.Database.GetCollection<Transport>("transport");
        }

        public async Task<List<Transport>> GetAllAsync() => await _transport.Find(_ => true).ToListAsync();
        public async Task<Transport> GetByIdAsync(string id) => await _transport.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Transport transport) => await _transport.InsertOneAsync(transport);
        public async Task UpdateAsync(string id, Transport transport) => await _transport.ReplaceOneAsync(x => x.Id == id, transport);
        public async Task DeleteAsync(string id) => await _transport.DeleteOneAsync(x => x.Id == id);
    }
}
