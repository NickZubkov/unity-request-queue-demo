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
            if (!_isActive) return;

            if (result.Success)
            {
                _view.PlayTapVfx(result.Reward);
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
            _view.ClearVfx();
            _view.Hide();
        }

        public void Dispose()
        {
            _view.TapClicked -= _clicker.Tap;
            _disposables.Dispose();
        }
    }
}
