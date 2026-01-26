namespace AvesPipeline.WorkerService.Models;

public sealed class TaxonDto
{
    public int GbifId { get; init; }

    // Index page
    public string ScientificName { get; init; } = "";
    public string CanonicalName { get; init; } = "";
    public string Rank { get; init; } = "";
    public string Family { get; init; } = "";
    public string Order { get; init; } = "";

    // Details page
    public string Kingdom { get; init; } = "";
    public string Phylum { get; init; } = "";
    public string Class { get; init; } = "";
    public string Genus { get; init; } = "";
}