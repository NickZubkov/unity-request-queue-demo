using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RequestQueueDemo.App.Features.Weather
{
    public sealed class WeatherView : MonoBehaviour, IWeatherView
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private RawImage _icon;
        [SerializeField] private Texture2D _fallbackIcon;
        [SerializeField] private TMP_Text _text;

        public void Show() => _panel.SetActive(true);
        public void Hide() => _panel.SetActive(false);
        public void SetWeather(string text) => _text.text = text;
        public void SetIcon(Texture2D texture) => _icon.texture = texture;
        public void SetIconFallback() => _icon.texture = _fallbackIcon;
    }
}
