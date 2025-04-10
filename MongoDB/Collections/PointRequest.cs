using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using best_hackathon_2025.MongoDB.Collections;


namespace best_hackathon_2025.MongoDB.Collections
{
    public class PointRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PointId {get; set;}

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set;}

    }
}
