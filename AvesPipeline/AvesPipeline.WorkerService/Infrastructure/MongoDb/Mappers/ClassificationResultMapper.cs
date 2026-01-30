using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.Utils;
using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Mappers;

public static class ClassificationResultMapper
{
    public static List<ClassificationResult> ToClassificationResult(ClassificationResponseDto? dto)
    {
        if (dto == null || dto.Results.Count == 0) return [];
        
        return dto.Results.Select(r =>
        {
            var (scientific, common) = ClassificationLabelParser.Parse(r.Label);

            return new ClassificationResult
            {
                ScientificName = scientific,
                CommonName = common,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Confidence = r.Confidence,
                Label = r.Label
            };
        }).ToList();
    }
}