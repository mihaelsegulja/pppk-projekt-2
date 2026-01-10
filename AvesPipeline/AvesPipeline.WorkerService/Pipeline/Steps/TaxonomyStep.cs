using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline.Steps
{
    public class TaxonomyStep : IPipelineStep
    {
        public string Name => "Taxonomy Step";

        public Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: fetch aves.regoch.net and insert into MongoDB
            return Task.CompletedTask;
        }
    }
}
