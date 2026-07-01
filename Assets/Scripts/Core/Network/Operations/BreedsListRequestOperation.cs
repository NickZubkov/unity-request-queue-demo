using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RequestQueueDemo.Core.Domain;
using RequestQueueDemo.Core.Network.Dto;
using RequestQueueDemo.Core.Queue;

namespace RequestQueueDemo.Core.Network.Operations
{
    public sealed class BreedsListRequestOperation : IRequestOperation<IReadOnlyList<Breed>>
    {
        private readonly IWebRequestRunner _runner;
        private readonly IApiConfig _config;

        public BreedsListRequestOperation(IWebRequestRunner runner, IApiConfig config)
        {
            _runner = runner; _config = config;
        }

        public async UniTask<IReadOnlyList<Breed>> ExecuteAsync(CancellationToken ct)
        {
            var json = await _runner.GetTextAsync(_config.BreedsListUrl, ct);
            var dto = JsonConvert.DeserializeObject<BreedsListResponse>(json);
            return BreedMapper.ToList(dto, _config.BreedsListLimit);
        }
    }
}
