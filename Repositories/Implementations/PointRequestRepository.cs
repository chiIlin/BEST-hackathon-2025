using best_hackathon_2025.MongoDB;
using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class PointRequestRepository : IPointRequestRepository
    {
        private readonly IMongoCollection<PointRequest> _pointRequests;

        public PointRequestRepository(MongoDbContext context)
        {
            _pointRequests = context.Database.GetCollection<PointRequest>("pointRequests");
        }

        public async Task<List<PointRequest>> GetAllAsync()
            => await _pointRequests.Find(_ => true).ToListAsync();

        public async Task<PointRequest> GetByIdAsync(string id)
            => await _pointRequests.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(PointRequest pointRequest)
            => await _pointRequests.InsertOneAsync(pointRequest);

        public async Task UpdateAsync(string id, PointRequest pointRequest)
            => await _pointRequests.ReplaceOneAsync(x => x.Id == id, pointRequest);

        public async Task DeleteAsync(string id)
            => await _pointRequests.DeleteOneAsync(x => x.Id == id);
    }
}
