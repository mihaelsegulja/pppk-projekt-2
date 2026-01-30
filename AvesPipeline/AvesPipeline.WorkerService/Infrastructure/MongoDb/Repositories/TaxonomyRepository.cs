using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Mappers;
using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;

public sealed class TaxonomyRepository : ITaxonomyRepository
{
    private readonly IMongoCollection<TaxonDocument> _collection;

    public TaxonomyRepository(IOptions<MongoDbOptions> options)
    {
        var cfg = options.Value;

        var client = new MongoClient(cfg.ConnectionString);
        var database = client.GetDatabase(cfg.Database);

        _collection = database.GetCollection<TaxonDocument>("taxonomy");

        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        var indexKeys = Builders<TaxonDocument>
            .IndexKeys
            .Ascending(x => x.GbifId);

        var indexModel = new CreateIndexModel<TaxonDocument>(
            indexKeys,
            new CreateIndexOptions { Unique = true });

        _collection.Indexes.CreateOne(indexModel);
    }

    public async Task<bool> AnyAsync(CancellationToken ct)
    {
        return await _collection
            .Find(FilterDefinition<TaxonDocument>.Empty)
            .Limit(1)
            .AnyAsync(ct);
    }

    public async Task InsertManyAsync(IReadOnlyCollection<TaxonDto> taxa, CancellationToken ct)
    {
        if (taxa.Count == 0)
            return;

        var documents = taxa
            .Select(TaxonMapper.ToDocument)
            .Distinct()
            .ToList();

        await _collection.InsertManyAsync(
            documents,
            new InsertManyOptions { IsOrdered = false },
            ct);
    }
    
    public async Task<List<TaxonDocument>> GetAllAsync(CancellationToken ct)
    {
        return await _collection.Find(_ => true).ToListAsync(ct);
    }
}