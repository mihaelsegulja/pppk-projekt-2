using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvesPipeline.WorkerService.Pipeline;

public sealed class PipelineRunner : IPipelineRunner
{
    private readonly IEnumerable<IPipelineStep> _steps;
    private readonly ILogger<PipelineRunner> _logger;

    public PipelineRunner(IEnumerable<IPipelineStep> steps, ILogger<PipelineRunner> logger)
    {
        _steps = steps;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        foreach (var step in _steps)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("Starting step: {Step}", step.Name);
            await step.RunAsync(cancellationToken);
            _logger.LogInformation("Finished step: {Step} in {Elapsed}", step.Name, sw.Elapsed);
        }
    }
}
