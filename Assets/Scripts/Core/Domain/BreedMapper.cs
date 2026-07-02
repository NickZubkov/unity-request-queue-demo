using System;
using System.Collections.Generic;
using System.Linq;
using RequestQueueDemo.Core.Network.Dto;

namespace RequestQueueDemo.Core.Domain
{
    public static class BreedMapper
    {
        public static IReadOnlyList<Breed> ToList(BreedsListResponse dto, int limit)
        {
            if (dto?.Data == null) return Array.Empty<Breed>();
            return dto.Data
                .Where(d => d?.Attributes != null)
                .Take(limit)
                .Select(d => new Breed(d.Id, d.Attributes.Name))
                .ToList();
        }

        public static BreedFacts ToFacts(BreedDetailResponse dto)
        {
            var attributes = dto?.Data?.Attributes;
            if (attributes == null)
                throw new InvalidOperationException("Breed detail response has no attributes");
            return new BreedFacts(attributes.Name, attributes.Description);
        }
    }
}
