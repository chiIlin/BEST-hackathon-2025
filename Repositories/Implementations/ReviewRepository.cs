using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.MongoDB;
using best_hackathon_2025.Repositories.Interfaces;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IMongoCollection<Review> _reviews;

        public ReviewRepository(MongoDbContext context)
        {
            _reviews = context.Database.GetCollection<Review>("Reviews");
        }

        public async Task<List<Review>> GetAllAsync() => await _reviews.Find(_ => true).ToListAsync();
        public async Task<Review> GetByIdAsync(string id) => await _reviews.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Review review) => await _reviews.InsertOneAsync(review);
        public async Task UpdateAsync(string id, Review review) => await _reviews.ReplaceOneAsync(x => x.Id == id, review);
        public async Task DeleteAsync(string id) => await _reviews.DeleteOneAsync(x => x.Id == id);
    }
}
