using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline.Steps
{
    public class StatisticsStep : IPipelineStep
    {
        public string Name => nameof(StatisticsStep);

        public Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Implement statistics calculation logic
            return Task.CompletedTask;
        }
    }
}
