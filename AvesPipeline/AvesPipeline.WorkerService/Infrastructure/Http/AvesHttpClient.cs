using System.Text.Json;
using AvesPipeline.WorkerService.Infrastructure.Http.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.Http;

public sealed class AvesHttpClient : IAvesHttpClient
{
    private readonly HttpClient _http;

    public AvesHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<AvesSpeciesDto>> GetAllSpeciesAsync(CancellationToken ct)
    {
        var response = await _http.GetAsync("https://aves.regoch.net/aves.json", ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        var data = await JsonSerializer.DeserializeAsync<List<AvesSpeciesDto>>(
            stream,
            JsonSerializerOptions,
            ct);

        return data ?? [];
    }

    public async Task<GbifSpeciesDto?> GetSpeciesDetailsAsync(int speciesKey, CancellationToken ct)
    {
        var response = await _http.GetAsync(
            $"https://api.gbif.org/v1/species/{speciesKey}", ct);

        if (!response.IsSuccessStatusCode)
            return null;

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        return await JsonSerializer.DeserializeAsync<GbifSpeciesDto>(
            stream,
            JsonSerializerOptions,
            ct);
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions =
        new(JsonSerializerDefaults.Web);
}
