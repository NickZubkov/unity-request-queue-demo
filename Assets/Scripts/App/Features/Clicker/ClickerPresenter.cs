using System;
using RequestQueueDemo.App.Navigation;
using UniRx;
using Zenject;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class ClickerPresenter : ITab, IInitializable, IDisposable
    {
        private readonly IClickerView _view;
        private readonly CurrencyModel _currency;
        private readonly EnergyModel _energy;
        private readonly ClickerService _clicker;
        private readonly CompositeDisposable _disposables = new();
        private bool _isActive;

        public TabId Id => TabId.Clicker;

        public ClickerPresenter(IClickerView view, CurrencyModel currency, EnergyModel energy, ClickerService clicker)
        {
            _view = view; _currency = currency; _energy = energy; _clicker = clicker;
        }

        public void Initialize()
        {
            _currency.Amount.Subscribe(v => _view.SetCurrency(v)).AddTo(_disposables);
            _energy.Energy.Subscribe(v => _view.SetEnergy(v, _energy.Max)).AddTo(_disposables);
            _clicker.Taps.Subscribe(OnTap).AddTo(_disposables);
            _view.TapClicked += _clicker.Tap;
        }

        private void OnTap(TapResult result)
        {
            // Вне вкладки кликера VFX не проигрываем: автосбор тикает фоном на любой вкладке,
            // а «+1»/партиклы спавнятся под Canvas и иначе были бы видны поверх другой вкладки.
            // Счётчики при этом обновляются подписками выше — фоновое накопление сохраняется.
            if (!_isActive) return;

            if (result == TapResult.Success)
            {
                _view.PlayTapVfx();
                _view.PlayButtonPunch();
            }
            else
            {
                _view.PlayNoEnergyFeedback();
            }
        }

        public void OnEnter()
        {
            _isActive = true;
            _view.Show();
        }

        public void OnExit()
        {
            _isActive = false;
            _view.ClearVfx(); // гасим «в полёте» — активные «+1»/партиклы возвращаются в пул
            _view.Hide();
        }

        public void Dispose()
        {
            _view.TapClicked -= _clicker.Tap;
            _disposables.Dispose();
        }
    }
}
