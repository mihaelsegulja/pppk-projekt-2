using AvesPipeline.WorkerService;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;
using AvesPipeline.WorkerService.Infrastructure.S3;
using AvesPipeline.WorkerService.Infrastructure.Web;
using AvesPipeline.WorkerService.Options;
using AvesPipeline.WorkerService.Pipeline;
using AvesPipeline.WorkerService.Pipeline.Steps;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<WebOptions>(builder.Configuration.GetSection(WebOptions.SectionName));
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(MongoDbOptions.SectionName));
builder.Services.Configure<S3Options>(builder.Configuration.GetSection(S3Options.SectionName));
builder.Services.Configure<AudioProcessingStepOptions>(builder.Configuration.GetSection(AudioProcessingStepOptions.SectionName));

builder.Services.AddSingleton<ITaxonomyScraper, TaxonomyScraper>();
builder.Services.AddHttpClient<IBirdClassifier, BirdClassifier>();

builder.Services.AddSingleton<ITaxonomyRepository, TaxonomyRepository>();
builder.Services.AddSingleton<IAudioRepository, AudioRepository>();

builder.Services.AddSingleton<IS3ObjectStorage, S3ObjectStorage>();

builder.Services.AddSingleton<IPipelineRunner, PipelineRunner>();
builder.Services.AddSingleton<IPipelineStep, TaxonomyStep>();
builder.Services.AddSingleton<IPipelineStep, AudioProcessingStep>();
builder.Services.AddSingleton<IPipelineStep, StatisticsStep>();

var host = builder.Build();
host.Run();
