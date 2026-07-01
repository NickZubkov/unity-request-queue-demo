using UniRx;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class CurrencyModel
    {
        private readonly ReactiveProperty<long> _amount = new(0);
        public IReadOnlyReactiveProperty<long> Amount => _amount;
        public void Add(long value) => _amount.Value += value;
    }
}
