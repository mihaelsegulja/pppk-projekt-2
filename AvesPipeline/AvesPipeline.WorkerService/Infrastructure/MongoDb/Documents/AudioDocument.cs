using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;

public sealed class AudioDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = null!;

    public string OriginalFileName { get; init; } = "";
    public string ContentType { get; init; } = "";

    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public string AudioObjectKey { get; init; } = "";
    public string ClassificationLogObjectKey { get; init; } = "";

    public List<ClassificationResult> Classifications { get; init; } = [];

    public DateTime ProcessedAtUtc { get; init; }
}

public sealed class ClassificationResult
{
    public string CommonName { get; init; } = "";
    public string ScientificName { get; init; } = "";

    public double StartTime { get; init; }
    public double EndTime { get; init; }

    public double Confidence { get; init; }

    public string Label { get; init; } = "";
}