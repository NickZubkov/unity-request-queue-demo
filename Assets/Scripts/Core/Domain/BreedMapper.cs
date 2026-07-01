using System.Collections.Generic;
using System.Linq;
using RequestQueueDemo.Core.Network.Dto;

namespace RequestQueueDemo.Core.Domain
{
    public static class BreedMapper
    {
        public static IReadOnlyList<Breed> ToList(BreedsListResponse dto, int limit) =>
            dto.Data.Take(limit).Select(d => new Breed(d.Id, d.Attributes.Name)).ToList();

        public static BreedFacts ToFacts(BreedDetailResponse dto) =>
            new BreedFacts(dto.Data.Attributes.Name, dto.Data.Attributes.Description);
    }
}
