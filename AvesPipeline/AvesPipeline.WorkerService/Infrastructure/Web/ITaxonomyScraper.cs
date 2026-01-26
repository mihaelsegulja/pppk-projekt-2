using AvesPipeline.WorkerService.Models;

namespace AvesPipeline.WorkerService.Infrastructure.Web;

public interface ITaxonomyScraper
{
    Task<IReadOnlyList<TaxonDto>> ScrapeAsync(CancellationToken ct);
}
