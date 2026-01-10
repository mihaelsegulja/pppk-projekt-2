using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline;

public interface IPipelineStep
{
    string Name { get; }
    Task RunAsync(CancellationToken cancellationToken);
}
