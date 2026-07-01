using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RequestQueueDemo.Core.Domain;
using RequestQueueDemo.Core.Network.Dto;
using RequestQueueDemo.Core.Queue;

namespace RequestQueueDemo.Core.Network.Operations
{
    public sealed class WeatherRequestOperation : IRequestOperation<WeatherInfo>
    {
        private readonly IWebRequestRunner _runner;
        private readonly IApiConfig _config;

        public WeatherRequestOperation(IWebRequestRunner runner, IApiConfig config)
        {
            _runner = runner; _config = config;
        }

        public async UniTask<WeatherInfo> ExecuteAsync(CancellationToken ct)
        {
            var json = await _runner.GetTextAsync(_config.WeatherUrl, ct);
            var dto = JsonConvert.DeserializeObject<WeatherForecastResponse>(json);
            return WeatherMapper.ToDomain(dto);
        }
    }
}
