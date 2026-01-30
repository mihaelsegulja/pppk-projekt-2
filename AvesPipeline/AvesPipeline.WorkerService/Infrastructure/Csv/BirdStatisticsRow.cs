namespace AvesPipeline.WorkerService.Infrastructure.Csv;

public sealed class BirdStatisticsRow
{
    public string ScientificName { get; init; }
    public string CommonName { get; init; }

    public int ObservationCount { get; init; }

    public double AverageConfidence { get; init; }
    public double MinConfidence { get; init; }
    public double MaxConfidence { get; init; }

    public string Family { get; init; }
    public string Order { get; init; }
    public string Genus { get; init; }
}