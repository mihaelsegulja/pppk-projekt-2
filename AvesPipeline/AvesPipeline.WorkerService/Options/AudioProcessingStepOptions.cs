namespace AvesPipeline.WorkerService.Options;

public sealed class AudioProcessingStepOptions
{
    public const string SectionName = "Steps:AudioProcessingStep";

    public string InputDirectory { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}