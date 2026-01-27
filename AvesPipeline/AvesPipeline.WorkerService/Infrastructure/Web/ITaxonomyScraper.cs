using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.Web;

public interface ITaxonomyScraper
{
    Task<IReadOnlyList<TaxonDto>> ScrapeAsync(CancellationToken ct);
}
