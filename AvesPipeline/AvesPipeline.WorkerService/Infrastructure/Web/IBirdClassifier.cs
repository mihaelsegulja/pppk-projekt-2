using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.Web;

public interface IBirdClassifier
{
    Task<ClassificationResponseDto?> ClassifyAsync(Stream audio, string fileName, CancellationToken ct);
}