using System.Net.Http.Headers;
using System.Text.Json;
using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;

namespace AvesPipeline.WorkerService.Infrastructure.Web;

public class BirdClassifier : IBirdClassifier
{
    private readonly HttpClient _http;
    private readonly WebOptions _options;

    public BirdClassifier(HttpClient http, IOptions<WebOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<ClassificationResponseDto?> ClassifyAsync(
        Stream audio,
        string fileName,
        CancellationToken ct)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(audio);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

        content.Add(fileContent, "file", fileName);

        var response = await _http.PostAsync($"{_options.BaseUrl}/api/classify", content, ct);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync(ct);

        return JsonSerializer.Deserialize<ClassificationResponseDto>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ClassificationResponseDto();
    }
}