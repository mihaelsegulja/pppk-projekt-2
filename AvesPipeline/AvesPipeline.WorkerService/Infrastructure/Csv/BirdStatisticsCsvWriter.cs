using System.Text;

namespace AvesPipeline.WorkerService.Infrastructure.Csv;

public sealed class BirdStatisticsCsvWriter : ICsvWriter<BirdStatisticsRow>
{
    public async Task WriteAsync(IEnumerable<BirdStatisticsRow> rows, string filePath, CancellationToken ct)
    {
        var sb = new StringBuilder();

        sb.AppendLine("ScientificName,CommonName,ObservationCount,AvgConfidence,MinConfidence,MaxConfidence,Family,Order,Genus");

        foreach (var r in rows)
        {
            sb.AppendLine(
                $"{Escape(r.ScientificName)}," +
                $"{Escape(r.CommonName)}," +
                $"{r.ObservationCount}," +
                $"{r.AverageConfidence:F4}," +
                $"{r.MinConfidence:F4}," +
                $"{r.MaxConfidence:F4}," +
                $"{Escape(r.Family)}," +
                $"{Escape(r.Order)}," +
                $"{Escape(r.Genus)}");
        }

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await File.WriteAllTextAsync(filePath, sb.ToString(), ct);
    }

    private static string Escape(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";

        return value;
    }
}