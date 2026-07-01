namespace RequestQueueDemo.Core.Domain
{
    public sealed class WeatherInfo
    {
        public string Label { get; }
        public int Temperature { get; }
        public string Unit { get; }
        public string IconUrl { get; }

        public WeatherInfo(string label, int temperature, string unit, string iconUrl)
        {
            Label = label; Temperature = temperature; Unit = unit; IconUrl = iconUrl;
        }

        public string Display => $"{Label} - {Temperature}{Unit}";
    }
}
