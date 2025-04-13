using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public List<string> Points { get; set; }
    }
}
