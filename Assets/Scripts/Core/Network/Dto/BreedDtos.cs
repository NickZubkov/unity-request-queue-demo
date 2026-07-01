using Newtonsoft.Json;

namespace RequestQueueDemo.Core.Network.Dto
{
    public sealed class BreedsListResponse
    {
        [JsonProperty("data")] public BreedData[] Data { get; set; }
    }

    public sealed class BreedDetailResponse
    {
        [JsonProperty("data")] public BreedData Data { get; set; }
    }

    public sealed class BreedData
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("attributes")] public BreedAttributes Attributes { get; set; }
    }

    public sealed class BreedAttributes
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
    }
}
