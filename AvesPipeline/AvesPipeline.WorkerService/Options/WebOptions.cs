namespace AvesPipeline.WorkerService.Options;

public sealed class WebOptions
{
    public const string SectionName = "Infrastructure:Web";

    public string BaseUrl { get; init; }
    public int MaxParallelDetailsProcessing { get; init; }
}
