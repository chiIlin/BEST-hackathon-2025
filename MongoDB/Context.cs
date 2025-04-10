using Microsoft.AspNetCore.Mvc.ViewEngines;
using MongoDB.Driver;
using System.Security.Cryptography.Xml;
using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            _database = client.GetDatabase("InclusiveMap");

        }
        public IMongoDatabase Database => _database;


        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Point> Points => _database.GetCollection<Point>("points");
        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("reviews");
        public IMongoCollection<Transport> Transport => _database.GetCollection<Transport>("transport");
        public IMongoCollection<PointRequest> PointRequest => _database.GetCollection<PointRequest>("pointRequests");
    }
}
