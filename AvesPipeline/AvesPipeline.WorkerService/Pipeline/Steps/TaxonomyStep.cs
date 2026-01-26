using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvesPipeline.WorkerService.Infrastructure.Web;

namespace AvesPipeline.WorkerService.Pipeline.Steps;

public class TaxonomyStep : IPipelineStep
{
    private readonly ITaxonomyScraper _scraper;
    private readonly ILogger<TaxonomyStep> _logger;

    public TaxonomyStep(
        ITaxonomyScraper scraper,
        ILogger<TaxonomyStep> logger)
    {
        _scraper = scraper;
        _logger = logger;
    }
    
    public string Name => nameof(TaxonomyStep);

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting taxonomy step");
        var rows = await _scraper.ScrapeAsync(cancellationToken);
        _logger.LogInformation("Found {Count} rows", rows.Count);
    }
}