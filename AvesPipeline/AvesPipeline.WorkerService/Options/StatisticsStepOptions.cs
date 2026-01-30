namespace AvesPipeline.WorkerService.Options;

public class StatisticsStepOptions
{
    public const string SectionName = "Steps:StatisticsStep";

    public string OutputDirectory { get; init; }
    public double MinConfidence { get; init; }
    public string? SpeciesFilter { get; init; }
}