namespace AvesPipeline.WorkerService.Options;

public sealed class S3Options
{
    public const string SectionName = "Infrastructure:S3";

    public string BaseUrl { get; init; }
    public string Bucket { get; init; }
}
