namespace AvesPipeline.WorkerService.Infrastructure.S3;

public interface IS3ObjectStorage
{
    Task<S3ObjectRef> UploadAsync(
        Stream content,
        string key,
        string contentType,
        IDictionary<string, string>? metadata,
        CancellationToken ct);

    Task<Stream> DownloadAsync(
        string key,
        CancellationToken ct);
}