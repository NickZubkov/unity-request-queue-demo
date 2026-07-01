using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RequestQueueDemo.Core.Domain;
using RequestQueueDemo.Core.Network.Dto;
using RequestQueueDemo.Core.Queue;

namespace RequestQueueDemo.Core.Network.Operations
{
    public sealed class BreedFactsRequestOperation : IRequestOperation<BreedFacts>
    {
        private readonly IWebRequestRunner _runner;
        private readonly IApiConfig _config;
        private readonly string _breedId;

        public BreedFactsRequestOperation(IWebRequestRunner runner, IApiConfig config, string breedId)
        {
            _runner = runner; _config = config; _breedId = breedId;
        }

        public async UniTask<BreedFacts> ExecuteAsync(CancellationToken ct)
        {
            var json = await _runner.GetTextAsync(_config.BreedDetailUrl(_breedId), ct);
            var dto = JsonConvert.DeserializeObject<BreedDetailResponse>(json);
            return BreedMapper.ToFacts(dto);
        }
    }
}
