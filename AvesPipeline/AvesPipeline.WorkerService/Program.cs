using AvesPipeline.WorkerService;
using AvesPipeline.WorkerService.Pipeline;
using AvesPipeline.WorkerService.Pipeline.Steps;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IPipelineRunner, PipelineRunner>();

builder.Services.AddSingleton<IPipelineStep, TaxonomyStep>();
builder.Services.AddSingleton<IPipelineStep, ObservationStep>();
builder.Services.AddSingleton<IPipelineStep, AudioProcessingStep>();
builder.Services.AddSingleton<IPipelineStep, StatisticsStep>();

var host = builder.Build();
host.Run();
