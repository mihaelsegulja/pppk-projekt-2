namespace AvesPipeline.WorkerService.Infrastructure.Http.Dtos;

public record AvesSpeciesDto
{
    public int Key { get; init; }
    public int? NubKey { get; init; }

    public string? ScientificName { get; init; }
    public string? CanonicalName { get; init; }
    public string? Authorship { get; init; }

    public string? Rank { get; init; }
    public string? TaxonomicStatus { get; init; }

    public string? Kingdom { get; init; }
    public string? Phylum { get; init; }
    public string? Class { get; init; }
    public string? Order { get; init; }
    public string? Family { get; init; }
    public string? Genus { get; init; }
    public string? Species { get; init; }

    public bool? Extinct { get; init; }

    public IReadOnlyList<string>? ThreatStatuses { get; init; }
    public IReadOnlyList<DescriptionDto>? Descriptions { get; init; }
    public IReadOnlyList<VernacularNameDto>? VernacularNames { get; init; }

    public IReadOnlyDictionary<string, string>? HigherClassificationMap { get; init; }

    public bool? Synonym { get; init; }
}

public record DescriptionDto
{
    public string? Description { get; init; }
}

public record VernacularNameDto
{
    public string? VernacularName { get; init; }
    public string? Language { get; init; }
}

public record GbifSpeciesDto
{
    public int Key { get; init; }
    public int? NubKey { get; init; }

    public string? ScientificName { get; init; }
    public string? CanonicalName { get; init; }
    public string? VernacularName { get; init; }
    public string? Authorship { get; init; }

    public string? Rank { get; init; }
    public string? TaxonomicStatus { get; init; }
    public string? NameType { get; init; }
    public string? Origin { get; init; }

    public string? Kingdom { get; init; }
    public string? Phylum { get; init; }
    public string? Class { get; init; }
    public string? Order { get; init; }
    public string? Family { get; init; }
    public string? Genus { get; init; }
    public string? Species { get; init; }

    public int? ParentKey { get; init; }
    public string? Parent { get; init; }

    public string? DatasetKey { get; init; }

    public DateTimeOffset? LastCrawled { get; init; }
    public DateTimeOffset? LastInterpreted { get; init; }

    public IReadOnlyList<string>? Issues { get; init; }
}
