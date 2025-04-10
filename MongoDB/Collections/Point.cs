using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class Point
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Location { get; set; }
        public List<string> Categories { get; set; }
        public List<string> ReviewIds { get; set; }
        public double Rating { get; set; }
        public int LOI { get; set; } // Level Of Inclusiveness
        public string Adress { get; set; }
        public string Description { get; set; }
        public bool Verified { get; set; }
    }

}
