namespace RequestQueueDemo.Core.Network
{
    public interface IApiConfig
    {
        public string WeatherUrl { get; }
        public string BreedsListUrl { get; }
        public string UserAgent { get; }
        public string AcceptHeader { get; }
        public int WeatherPollIntervalSeconds { get; }
        public int BreedsListLimit { get; }
        public string BreedDetailUrl(string breedId);
    }
}
