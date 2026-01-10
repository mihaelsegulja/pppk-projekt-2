using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline.Steps
{
    public class AudioProcessingStep : IPipelineStep
    {
        public string Name => "Audio Processing Step";

        public Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Implement audio processing logic
            return Task.CompletedTask;
        }
    }
}
