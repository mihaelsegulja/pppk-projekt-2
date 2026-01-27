using AvesPipeline.WorkerService.Infrastructure.MongoDb.Documents;
using AvesPipeline.WorkerService.Infrastructure.Web.Dtos;

namespace AvesPipeline.WorkerService.Infrastructure.MongoDb.Mappers;

public static class TaxonMapper
{
    public static TaxonDocument ToDocument(TaxonDto dto)
    {
        return new TaxonDocument
        {
            GbifId = dto.GbifId,
            ScientificName = dto.ScientificName,
            CanonicalName = dto.CanonicalName,
            Rank = dto.Rank,
            Family = dto.Family,
            Order = dto.Order,
            Kingdom = dto.Kingdom,
            Phylum = dto.Phylum,
            Class = dto.Class,
            Genus = dto.Genus
        };
    }
}