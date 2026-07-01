using System;
using RequestQueueDemo.App.Config;
using UniRx;
using Zenject;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class AutoCollectService : IInitializable, IDisposable
    {
        private readonly ClickerService _clicker;
        private readonly ClickerConfig _config;
        private IDisposable _sub;

        public AutoCollectService(ClickerService clicker, ClickerConfig config)
        {
            _clicker = clicker; _config = config;
        }

        public void Initialize() =>
            _sub = Observable.Interval(TimeSpan.FromSeconds(_config.AutoCollectIntervalSeconds))
                             .Subscribe(_ => _clicker.AutoCollect());

        public void Dispose() => _sub?.Dispose();
    }
}
