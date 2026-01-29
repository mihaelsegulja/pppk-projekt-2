using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Mappers;
using AvesPipeline.WorkerService.Infrastructure.MongoDb.Repositories;
using AvesPipeline.WorkerService.Infrastructure.S3;
using AvesPipeline.WorkerService.Infrastructure.Web;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;

namespace AvesPipeline.WorkerService.Pipeline.Steps;

public sealed class AudioProcessingStep : IPipelineStep
{
    private readonly IS3ObjectStorage _storage;
    private readonly IAudioRepository _repository;
    private readonly IBirdClassifier _classifier;
    private readonly AudioProcessingStepOptions _options;
    private readonly ILogger<AudioProcessingStep> _logger;

    public AudioProcessingStep(
        IS3ObjectStorage storage,
        IAudioRepository repository,
        IBirdClassifier classifier,
        IOptions<AudioProcessingStepOptions> options,
        ILogger<AudioProcessingStep> logger)
    {
        _storage = storage;
        _repository = repository;
        _classifier = classifier;
        _options = options.Value;
        _logger = logger;
    }

    public string Name => nameof(AudioProcessingStep);

    public async Task RunAsync(CancellationToken ct)
    {
        var files = Directory.GetFiles(_options.InputDirectory);

        foreach (var path in files)
        {
            ct.ThrowIfCancellationRequested();

            var fileName = Path.GetFileName(path);
            var objectKey = $"audio/{fileName}";

            if (await _repository.ExistsAsync(objectKey, ct))
            {
                _logger.LogInformation("Skipping {File}", fileName);
                continue;
            }

            await using var fs = File.OpenRead(path);

            // Upload audio
            await _storage.UploadAsync(
                fs,
                objectKey,
                "audio/mp3",
                metadata: null,
                ct);
            
            // Classify
            await using var classifyStream = File.OpenRead(path);
            var result = await _classifier.ClassifyAsync(classifyStream, fileName, ct);

            var logJson = System.Text.Json.JsonSerializer.Serialize(result);
            await using var logStream =
                new MemoryStream(Encoding.UTF8.GetBytes(logJson));

            var logKey = $"classification-logs/{fileName}.json";

            await _storage.UploadAsync(
                logStream,
                logKey,
                "application/json",
                metadata: null,
                ct);

            var document = new AudioDocument
            {
                OriginalFileName = fileName,
                ContentType = "audio/mp3",
                Latitude = _options.Latitude,
                Longitude = _options.Longitude,
                AudioObjectKey = objectKey,
                ClassificationLogObjectKey = logKey,
                Classifications = ClassificationResultMapper.ToClassificationResult(result),
                ProcessedAtUtc = DateTime.UtcNow
            };

            await _repository.InsertAsync(document, ct);
        }
    }
}