using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;
using AvesPipeline.WorkerService.Infrastructure.Web;

namespace AvesPipeline.WorkerService.Pipeline.Steps;

public class TaxonomyStep : IPipelineStep
{
    private readonly ITaxonomyScraper _scraper;
    private readonly ITaxonomyRepository _repository;
    private readonly ILogger<TaxonomyStep> _logger;
    

    public TaxonomyStep(
        ITaxonomyScraper scraper,
        ITaxonomyRepository taxonomyRepository,
        ILogger<TaxonomyStep> logger)
    {
        _scraper = scraper;
        _repository = taxonomyRepository;
        _logger = logger;
    }
    
    public string Name => nameof(TaxonomyStep);

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting taxonomy step");

        var exists = await _repository.AnyAsync(cancellationToken);
        if (exists)
        {
            _logger.LogInformation(
                "Taxonomy data already exists, skipping scraping step");
            return;
        }

        _logger.LogInformation("No taxonomy data found, starting scrape");

        var rows = await _scraper.ScrapeAsync(cancellationToken);

        _logger.LogInformation("Scraped {Count} taxonomy rows", rows.Count);

        await _repository.InsertManyAsync(rows, cancellationToken);

        _logger.LogInformation("Taxonomy data successfully stored in MongoDB");
    }
}