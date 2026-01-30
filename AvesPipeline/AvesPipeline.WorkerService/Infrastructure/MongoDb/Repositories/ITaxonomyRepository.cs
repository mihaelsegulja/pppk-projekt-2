using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;

public interface ITaxonomyRepository
{
    Task<bool> AnyAsync(CancellationToken ct);
    Task InsertManyAsync(IReadOnlyCollection<TaxonDto> taxa, CancellationToken ct);
    Task<List<TaxonDocument>> GetAllAsync(CancellationToken ct);
}
