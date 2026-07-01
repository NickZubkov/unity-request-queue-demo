using RequestQueueDemo.Core.Network;
using UnityEngine;

namespace RequestQueueDemo.App.Config
{
    [CreateAssetMenu(menuName = "RequestQueueDemo/ApiConfig", fileName = "ApiConfig")]
    public sealed class ApiConfig : ScriptableObject, IApiConfig
    {
        [SerializeField] private string _weatherUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        [SerializeField] private string _breedsListUrl = "https://dogapi.dog/api/v2/breeds";
        [SerializeField] private string _breedDetailUrlTemplate = "https://dogapi.dog/api/v2/breeds/{0}";
        [SerializeField] private string _userAgent = "unity-request-queue-demo (contact: dev@example.com)";
        [SerializeField] private string _acceptHeader = "application/geo+json";
        [SerializeField] private int _weatherPollIntervalSeconds = 5;
        [SerializeField] private int _breedsListLimit = 10;

        public string WeatherUrl => _weatherUrl;
        public string BreedsListUrl => _breedsListUrl;
        public string UserAgent => _userAgent;
        public string AcceptHeader => _acceptHeader;
        public int WeatherPollIntervalSeconds => _weatherPollIntervalSeconds;
        public int BreedsListLimit => _breedsListLimit;
        public string BreedDetailUrl(string breedId) => string.Format(_breedDetailUrlTemplate, breedId);
    }
}
