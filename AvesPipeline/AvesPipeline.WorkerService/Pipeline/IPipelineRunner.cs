using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline;

public interface IPipelineRunner
{
    Task RunAsync(CancellationToken cancellationToken);
}
