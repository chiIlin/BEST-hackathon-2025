using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace best_hackathon_2025.MongoDB.Collections
{
    public class LoiRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("pointId")]
        public string PointId { get; set; } = string.Empty;  // id поінта, до якого йде запит

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;   // хто подав запит

        [BsonElement("requestedLoi")]
        public int RequestedLoi { get; set; }                // який LOI хоче поставити

        [BsonElement("TimeCreated")]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow; // дата створення запиту

        [BsonElement("Status")]
        public string Status { get; set; } = "pending";     // статус: pending / approved / rejected
    }
}
