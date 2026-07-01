using UnityEngine;

namespace RequestQueueDemo.App.Features.Weather
{
    public interface IWeatherView
    {
        public void Show();
        public void Hide();
        public void SetWeather(string text);
        public void SetIcon(Texture2D texture);
        public void SetIconFallback();
    }
}
