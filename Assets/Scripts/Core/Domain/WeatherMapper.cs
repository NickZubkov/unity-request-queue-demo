using System.Linq;
using RequestQueueDemo.Core.Network.Dto;

namespace RequestQueueDemo.Core.Domain
{
    public static class WeatherMapper
    {
        public static WeatherInfo ToDomain(WeatherForecastResponse dto)
        {
            var p = dto.Properties.Periods.First();
            return new WeatherInfo(p.Name, p.Temperature, p.Unit, p.Icon);
        }
    }
}
