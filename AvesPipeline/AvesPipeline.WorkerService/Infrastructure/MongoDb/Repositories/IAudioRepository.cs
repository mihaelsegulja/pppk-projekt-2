using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;

public interface IAudioRepository
{
    Task<bool> ExistsAsync(string audioObjectKey, CancellationToken ct);
    Task InsertAsync(AudioDocument document, CancellationToken ct);
}