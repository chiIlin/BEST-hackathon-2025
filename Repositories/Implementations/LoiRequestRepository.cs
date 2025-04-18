﻿using best_hackathon_2025.MongoDB;
using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using MongoDB.Driver;

namespace best_hackathon_2025.Repositories.Implementations
{
    public class LoiRequestRepository : ILoiRequestRepository
    {
        private readonly IMongoCollection<Point> _points;
        private readonly IMongoCollection<LoiRequest> _collection;

        public LoiRequestRepository(MongoDbContext context)
        {
            _collection = context.LoiRequest;
            _points = context.Points; // важливо!!!
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
        public async Task UpdatePointLoiAsync(string pointId, int newLoi)
        {
            var filter = Builders<Point>.Filter.Eq(x => x.Id, pointId);
            var update = Builders<Point>.Update.Set(x => x.LOI, newLoi);
            await _points.UpdateOneAsync(filter, update);
        }

        public async Task UpdatePointManualLoiAsync(string pointId, int newManualLoi)
        {
            var filter = Builders<Point>.Filter.Eq(x => x.Id, pointId);
            var update = Builders<Point>.Update.Set(x => x.ManualLOI, newManualLoi);
            await _points.UpdateOneAsync(filter, update);
        }

    }
}
