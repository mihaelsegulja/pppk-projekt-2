using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;

public sealed class TaxonDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    [BsonElement("gbifId")]
    public int GbifId { get; init; }

    // Index page
    [BsonElement("scientificName")]
    public string ScientificName { get; init; } = "";

    [BsonElement("canonicalName")]
    public string CanonicalName { get; init; } = "";

    [BsonElement("rank")]
    public string Rank { get; init; } = "";

    [BsonElement("family")]
    public string Family { get; init; } = "";

    [BsonElement("order")]
    public string Order { get; init; } = "";

    // Details page
    [BsonElement("kingdom")]
    public string Kingdom { get; init; } = "";

    [BsonElement("phylum")]
    public string Phylum { get; init; } = "";

    [BsonElement("class")]
    public string Class { get; init; } = "";

    [BsonElement("genus")]
    public string Genus { get; init; } = "";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
