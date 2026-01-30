namespace AvesPipeline.WorkerService.Infrastructure.Utils;

public static class ClassificationLabelParser
{
    public static (string scientific, string common) Parse(string label)
    {
        if (string.IsNullOrWhiteSpace(label))
            return ("", "");

        var parts = label.Split('_', 2);
        if (parts.Length != 2)
            return ("", "");

        return (parts[0].Trim(), parts[1].Trim());
    }
}
