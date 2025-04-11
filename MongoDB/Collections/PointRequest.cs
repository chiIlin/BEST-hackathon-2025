// MongoDB/Collections/PointRequest.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace best_hackathon_2025.MongoDB.Collections;

public class PointRequest
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    /* запропонована точка (embedded) */
    public ProposedPoint Proposed { get; set; } = null!;
}

public class ProposedPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Categories { get; set; } = [];
    public int LOI { get; set; }
}
