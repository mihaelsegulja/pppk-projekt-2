namespace AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

public sealed class ClassificationResponseDto
{
    public List<ClassificationResultDto> Results { get; init; } = [];
}

public sealed class ClassificationResultDto
{
    public string CommonName { get; init; } = "";
    public string ScientificName { get; init; } = "";
    public double StartTime { get; init; }
    public double EndTime { get; init; }
    public double Confidence { get; init; }
    public string Label { get; init; } = "";
}