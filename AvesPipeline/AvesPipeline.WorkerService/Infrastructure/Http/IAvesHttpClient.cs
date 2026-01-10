using AvesPipeline.WorkerService.Infrastructure.Http.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.Http;

public interface IAvesHttpClient
{
    Task<IReadOnlyList<AvesSpeciesDto>> GetAllSpeciesAsync(CancellationToken cancellationToken);

    Task<GbifSpeciesDto?> GetSpeciesDetailsAsync(int speciesKey, CancellationToken cancellationToken);
}
