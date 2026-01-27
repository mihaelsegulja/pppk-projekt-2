using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.Persistence.MongoDb.Repositories;

public interface ITaxonomyRepository
{
    Task<bool> AnyAsync(CancellationToken ct);
    Task InsertManyAsync(IReadOnlyCollection<TaxonDto> taxa, CancellationToken ct);
}
