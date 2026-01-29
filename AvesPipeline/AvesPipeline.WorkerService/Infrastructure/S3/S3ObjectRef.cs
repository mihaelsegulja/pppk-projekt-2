namespace AvesPipeline.WorkerService.Infrastructure.S3;

public sealed class S3ObjectRef
{
    public string Bucket { get; init; } = "";
    public string Key { get; init; } = "";
    public string ContentType { get; init; } = "";
}