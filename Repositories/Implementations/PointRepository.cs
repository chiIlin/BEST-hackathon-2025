using best_hackathon_2025.MongoDB;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class PointRepository : IPointRepository
    {
        private readonly IMongoCollection<Point> _points;

        public PointRepository(MongoDbContext context)
        {
            _points = context.Database.GetCollection<Point>("points");
        }

        public async Task<List<Point>> GetAllAsync()
            => await _points.Find(_ => true).ToListAsync();

        public async Task<Point> GetByIdAsync(string id)
            => await _points.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Point point)
            => await _points.InsertOneAsync(point);

        public async Task UpdateAsync(string id, Point point)
            => await _points.ReplaceOneAsync(x => x.Id == id, point);

        public async Task DeleteAsync(string id)
            => await _points.DeleteOneAsync(x => x.Id == id);
    }

}
