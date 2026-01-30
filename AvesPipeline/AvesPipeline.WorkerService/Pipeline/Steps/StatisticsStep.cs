using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AvesPipeline.WorkerService.Infrastructure.Csv;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;
using AvesPipeline.WorkerService.Infrastructure.Utils;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;

namespace AvesPipeline.WorkerService.Pipeline.Steps;

public sealed class StatisticsStep : IPipelineStep
{
    private readonly ILogger<StatisticsStep> _logger;
    private readonly StatisticsStepOptions _options;
    private readonly IAudioRepository _audioRepository;
    private readonly ITaxonomyRepository _taxonomyRepository;
    private readonly ICsvWriter<BirdStatisticsRow> _csvWriter;

    public StatisticsStep(
        ILogger<StatisticsStep> logger,
        IOptions<StatisticsStepOptions> options,
        IAudioRepository audioRepository,
        ITaxonomyRepository taxonomyRepository,
        ICsvWriter<BirdStatisticsRow> csvWriter)
    {
        _logger = logger;
        _options = options.Value;
        _audioRepository = audioRepository;
        _taxonomyRepository = taxonomyRepository;
        _csvWriter = csvWriter;
    }

    public string Name => nameof(StatisticsStep);

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        // Load all audio documents and taxa
        var audioDocs = await _audioRepository.GetAllAsync(cancellationToken);
        var taxa = await _taxonomyRepository.GetAllAsync(cancellationToken);

        _logger.LogInformation("Loaded {AudioCount} audio documents and {TaxonCount} taxa", audioDocs.Count, taxa.Count);

        // Compile species filter regex if provided
        Regex? speciesFilterRegex = null;
        if (!string.IsNullOrWhiteSpace(_options.SpeciesFilter))
            speciesFilterRegex = new Regex(_options.SpeciesFilter, RegexOptions.IgnoreCase);

        // Flatten all classifications with confidence >= MinConfidence
        var flattened = audioDocs
            .Where(d => d.Classifications.Count > 0)
            .SelectMany(d => d.Classifications)
            .Where(c => c.Confidence >= _options.MinConfidence)
            .Select(c =>
            {
                var scientificName = string.IsNullOrWhiteSpace(c.ScientificName)
                    ? ClassificationLabelParser.Parse(c.Label).scientific
                    : c.ScientificName;

                return new
                {
                    Classification = c,
                    ScientificName = scientificName
                };
            })
            .Where(x => speciesFilterRegex == null || speciesFilterRegex.IsMatch(x.ScientificName))
            .ToList();

        // Group by classification scientific name and aggregate statistics
        var rows = flattened
            .GroupBy(x => x.ScientificName)
            .Select(g =>
            {
                var classificationName = g.Key.Trim().ToLowerInvariant();

                // Match taxon by scientific name without splitting or assumptions
                var taxon = taxa.FirstOrDefault(t =>
                    t.ScientificName.Trim().Contains(classificationName, StringComparison.InvariantCultureIgnoreCase));

                return new BirdStatisticsRow
                {
                    ScientificName = g.Key,
                    CommonName = g.First().Classification.CommonName,
                    ObservationCount = g.Count(),
                    AverageConfidence = g.Average(x => x.Classification.Confidence),
                    MinConfidence = g.Min(x => x.Classification.Confidence),
                    MaxConfidence = g.Max(x => x.Classification.Confidence),
                    Family = taxon?.Family ?? "",
                    Order = taxon?.Order ?? "",
                    Genus = taxon?.Genus ?? ""
                };
            })
            .OrderByDescending(r => r.ObservationCount)
            .ToList();

        if (rows.Count == 0)
        {
            _logger.LogWarning("No statistics generated");
            return;
        }

        // Write CSV
        var filePath = Path.Combine(_options.OutputDirectory, $"bird_statistics_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
        await _csvWriter.WriteAsync(rows, filePath, cancellationToken);

        _logger.LogInformation("Statistics export finished. Generated {RowCount} rows at {Path}", rows.Count, filePath);
    }
}