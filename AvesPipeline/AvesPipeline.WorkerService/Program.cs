using AvesPipeline.WorkerService;
using AvesPipeline.WorkerService.Infrastructure.Persistence.MongoDb.Repositories;
using AvesPipeline.WorkerService.Infrastructure.Web;
using AvesPipeline.WorkerService.Options;
using AvesPipeline.WorkerService.Pipeline;
using AvesPipeline.WorkerService.Pipeline.Steps;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<TaxonomyScraperOptions>(
    builder.Configuration.GetSection(TaxonomyScraperOptions.SectionName));

builder.Services.Configure<MongoDbOptions>(
    builder.Configuration.GetSection(MongoDbOptions.SectionName));

builder.Services.Configure<S3Options>(
    builder.Configuration.GetSection(S3Options.SectionName));

builder.Services.AddSingleton<ITaxonomyScraper, TaxonomyScraper>();
builder.Services.AddSingleton<ITaxonomyRepository, TaxonomyRepository>();

builder.Services.AddSingleton<IPipelineRunner, PipelineRunner>();

builder.Services.AddSingleton<IPipelineStep, TaxonomyStep>();
builder.Services.AddSingleton<IPipelineStep, ObservationStep>();
builder.Services.AddSingleton<IPipelineStep, AudioProcessingStep>();
builder.Services.AddSingleton<IPipelineStep, StatisticsStep>();

var host = builder.Build();
host.Run();
