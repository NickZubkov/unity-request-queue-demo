using System;
using RequestQueueDemo.App.Config;
using UniRx;
using Zenject;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class EnergyRegenService : IInitializable, IDisposable
    {
        private readonly EnergyModel _energy;
        private readonly ClickerConfig _config;
        private IDisposable _sub;

        public EnergyRegenService(EnergyModel energy, ClickerConfig config)
        {
            _energy = energy; _config = config;
        }

        public void Initialize() =>
            _sub = Observable.Interval(TimeSpan.FromSeconds(_config.EnergyRegenIntervalSeconds))
                             .Subscribe(_ => _energy.Refill(_config.EnergyRegenAmount));

        public void Dispose() => _sub?.Dispose();
    }
}
