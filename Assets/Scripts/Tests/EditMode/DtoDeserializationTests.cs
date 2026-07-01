using Newtonsoft.Json;
using NUnit.Framework;
using RequestQueueDemo.Core.Domain;
using RequestQueueDemo.Core.Network.Dto;

namespace RequestQueueDemo.Tests.EditMode
{
    public sealed class DtoDeserializationTests
    {
        [Test]
        public void Weather_Maps_First_Period()
        {
            const string json = @"{""properties"":{""periods"":[
                {""name"":""Today"",""temperature"":93,""temperatureUnit"":""F"",
                 ""icon"":""https://x/icon"",""shortForecast"":""Sunny"",""isDaytime"":true}]}}";
            var dto = JsonConvert.DeserializeObject<WeatherForecastResponse>(json);
            var info = WeatherMapper.ToDomain(dto);
            Assert.AreEqual("Today - 93F", info.Display);
            Assert.AreEqual("https://x/icon", info.IconUrl);
        }

        [Test]
        public void Breeds_List_Takes_Limit_And_Maps_Name()
        {
            const string json = @"{""data"":[
                {""id"":""a"",""attributes"":{""name"":""Affenpinscher"",""description"":""d1""}},
                {""id"":""b"",""attributes"":{""name"":""Afghan Hound"",""description"":""d2""}}]}";
            var dto = JsonConvert.DeserializeObject<BreedsListResponse>(json);
            var list = BreedMapper.ToList(dto, 1);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Affenpinscher", list[0].Name);
            Assert.AreEqual("a", list[0].Id);
        }

        [Test]
        public void Breed_Detail_Maps_Name_And_Description()
        {
            const string json = @"{""data"":{""id"":""a"",""attributes"":{""name"":""Affenpinscher"",""description"":""Small dog""}}}";
            var dto = JsonConvert.DeserializeObject<BreedDetailResponse>(json);
            var facts = BreedMapper.ToFacts(dto);
            Assert.AreEqual("Affenpinscher", facts.Name);
            Assert.AreEqual("Small dog", facts.Description);
        }
    }
}
