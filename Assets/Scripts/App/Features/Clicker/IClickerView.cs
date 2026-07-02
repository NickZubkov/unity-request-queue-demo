using System;

namespace RequestQueueDemo.App.Features.Clicker
{
    public interface IClickerView
    {
        public event Action TapClicked;
        public void Show();
        public void Hide();
        public void SetCurrency(long amount);
        public void SetEnergy(int energy, int max);
        public void PlayTapVfx(long reward);
        public void PlayButtonPunch();
        public void PlayNoEnergyFeedback();
        public void ClearVfx();
    }
}
