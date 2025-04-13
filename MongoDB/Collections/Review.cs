using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PointId { get; set; }
        public string UserId { get; set; }
        public string ReviewText { get; set; }
        public double Rating { get; set; }
        public string TimeCreated { get; set; }
    }

}
