using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class UserRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }

}
