namespace AvesPipeline.WorkerService.Options;

public sealed class MongoDbOptions
{
    public const string SectionName = "Infrastructure:MongoDb";

    public string ConnectionString { get; init; }
    public string Database { get; init; }
}
