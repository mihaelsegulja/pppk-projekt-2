using AvesPipeline.WorkerService.Pipeline;

namespace AvesPipeline.WorkerService;

public sealed class Worker : BackgroundService
{
    private readonly IPipelineRunner _pipelineRunner;
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime _lifetime;

    public Worker(IPipelineRunner pipelineRunner, ILogger<Worker> logger, IHostApplicationLifetime lifetime)
    {
        _pipelineRunner = pipelineRunner;
        _logger = logger;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Aves pipeline started");
            await _pipelineRunner.RunAsync(stoppingToken);
            _logger.LogInformation("Aves pipeline finished successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Pipeline execution failed");
        }
        finally
        {
            _lifetime.StopApplication();
        }
    }
}
