using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;

public sealed class AudioRepository : IAudioRepository
{
    private readonly IMongoCollection<AudioDocument> _collection;

    public AudioRepository(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.Database);

        _collection = database.GetCollection<AudioDocument>("audio");
    }

    public async Task<bool> ExistsAsync(string audioObjectKey, CancellationToken ct)
    {
        return await _collection
            .Find(x => x.AudioObjectKey == audioObjectKey)
            .AnyAsync(ct);
    }

    public async Task InsertAsync(AudioDocument document, CancellationToken ct)
    {
        await _collection.InsertOneAsync(document, cancellationToken: ct);
    }
}