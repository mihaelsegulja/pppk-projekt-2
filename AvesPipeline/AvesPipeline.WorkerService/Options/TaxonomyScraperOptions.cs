namespace AvesPipeline.WorkerService.Options;

public sealed class TaxonomyScraperOptions
{
    public const string SectionName = "Infrastructure:TaxonomyScraper";

    public string BaseUrl { get; init; }
    public int MaxParallelDetails { get; init; }
}
