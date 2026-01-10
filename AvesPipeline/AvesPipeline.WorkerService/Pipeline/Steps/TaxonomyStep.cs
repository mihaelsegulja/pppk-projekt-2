using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvesPipeline.WorkerService.Infrastructure.Http;

namespace AvesPipeline.WorkerService.Pipeline.Steps;

public class TaxonomyStep : IPipelineStep
{
    private readonly IAvesHttpClient _avesHttp;

    public TaxonomyStep(IAvesHttpClient avesHttp)
    {
        _avesHttp = avesHttp;
    }
    
    public string Name => nameof(TaxonomyStep);

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var species = await _avesHttp.GetAllSpeciesAsync(cancellationToken);

        Console.WriteLine($"Fetched {species.Count} species");
            
        // TODO:
        // - deduplicate
        // - map to persistence model
        // - store in MongoDB
    }
}