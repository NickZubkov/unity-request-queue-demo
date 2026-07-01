using RequestQueueDemo.App.Config;
using UniRx;
using UnityEngine;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class EnergyModel
    {
        private readonly ReactiveProperty<int> _energy;
        private readonly int _max;

        public IReadOnlyReactiveProperty<int> Energy => _energy;
        public int Max => _max;

        public EnergyModel(ClickerConfig config)
        {
            _energy = new ReactiveProperty<int>(config.EnergyStart);
            _max = config.EnergyMax;
        }

        public bool TrySpend(int cost)
        {
            if (_energy.Value < cost) return false;
            _energy.Value -= cost;
            return true;
        }

        public void Refill(int amount) => _energy.Value = Mathf.Min(_energy.Value + amount, _max);
    }
}
