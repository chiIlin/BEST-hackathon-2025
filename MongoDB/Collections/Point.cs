using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class Point
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public double Latitude { get; set; }   // ← координати окремо
        public double Longitude { get; set; }

        public List<string> Categories { get; set; } = [];
        public double Rating { get; set; }
        public int LOI { get; set; }        // Level Of Inclusiveness
        public string Address { get; set; }
        public string Description { get; set; }
        public bool Verified { get; set; }
    }

}
