using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Mappers;

public static class ClassificationResultMapper
{
    public static List<ClassificationResult> ToClassificationResult(ClassificationResponseDto? dto)
    {
        if (dto == null || dto.Results.Count == 0) return [];

        return dto.Results.Select(r => new ClassificationResult
        {
            CommonName = r.CommonName,
            ScientificName = r.ScientificName,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Confidence = r.Confidence,
            Label = r.Label
        }).ToList();
    }
}