using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class Transport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Route { get; set; }
        public int Type { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Stops { get; set; }
    }

}
