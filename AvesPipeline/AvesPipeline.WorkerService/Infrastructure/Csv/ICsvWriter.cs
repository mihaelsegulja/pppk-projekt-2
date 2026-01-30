namespace AvesPipeline.WorkerService.Infrastructure.Csv;

public interface ICsvWriter<T>
{
    Task WriteAsync(IEnumerable<T> rows, string filePath, CancellationToken ct);
}
