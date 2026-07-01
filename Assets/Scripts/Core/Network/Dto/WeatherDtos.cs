using Newtonsoft.Json;

namespace RequestQueueDemo.Core.Network.Dto
{
    public sealed class WeatherForecastResponse
    {
        [JsonProperty("properties")] public WeatherProperties Properties { get; set; }
    }

    public sealed class WeatherProperties
    {
        [JsonProperty("periods")] public WeatherPeriod[] Periods { get; set; }
    }

    public sealed class WeatherPeriod
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("temperature")] public int Temperature { get; set; }
        [JsonProperty("temperatureUnit")] public string Unit { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
        [JsonProperty("shortForecast")] public string ShortForecast { get; set; }
        [JsonProperty("isDaytime")] public bool IsDaytime { get; set; }
    }
}
