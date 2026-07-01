using System;
using RequestQueueDemo.App.Config;
using UniRx;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class ClickerService
    {
        private readonly CurrencyModel _currency;
        private readonly EnergyModel _energy;
        private readonly ClickerConfig _config;
        private readonly Subject<TapResult> _taps = new();

        public IObservable<TapResult> Taps => _taps;

        public ClickerService(CurrencyModel currency, EnergyModel energy, ClickerConfig config)
        {
            _currency = currency; _energy = energy; _config = config;
        }

        public void Tap() => Perform(_config.TapEnergyCost, _config.TapReward);
        public void AutoCollect() => Perform(_config.AutoCollectEnergyCost, _config.AutoCollectReward);

        private void Perform(int cost, long reward)
        {
            if (!_energy.TrySpend(cost)) { _taps.OnNext(TapResult.NoEnergy); return; }
            _currency.Add(reward);
            _taps.OnNext(TapResult.Success);
        }
    }
}
