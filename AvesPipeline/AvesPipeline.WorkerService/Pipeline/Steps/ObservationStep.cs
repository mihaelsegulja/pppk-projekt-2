using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline.Steps
{
    public class ObservationStep : IPipelineStep
    {
        public string Name => nameof(ObservationStep);

        public Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Implement observation logic
            return Task.CompletedTask;
        }
    }
}
