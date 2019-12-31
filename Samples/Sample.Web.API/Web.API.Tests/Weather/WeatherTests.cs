using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Sample.API.Controllers;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Samples.Web.API.Tests.WAPIT.Assertions;
using Synergy.Samples.Web.API.Tests.WAPIT.Features;

namespace Synergy.Samples.Web.API.Tests.Weather
{
    [TestFixture]
    public class WeatherTests
    {
        private const string Path = @"../../../Weather";
        private readonly Feature feature = new Feature("Manage weather through API");

        [Test]
        public void get_weather()
        {
            // ARRANGE
            var testServer = new SampleTestServer();
            var weather = new WeatherClient(testServer);
            testServer.Repair = false;

            // SCENARIO
            GetWeatherForecast(weather);
            CreateItem(weather);

            if (testServer.Repair)
            {
                new Markdown(feature).GenerateReportTo(Path + "/Weather.md");
            }
        }

        private void GetWeatherForecast(WeatherClient weather)
        {
            var scenario = feature.Scenario("Get weather forecast");

            weather.GetWeatherForecast()
                   .InStep(scenario.Step("Retrieve weather forecast"))
                   .ShouldBe(EqualToPattern("/Patterns/GetWeatherForecast.json")
                            .Ignore("$.response.body")
                            .Expected("Weather forecast is returned"))
                   .ShouldBe(InStatus(HttpStatusCode.OK));
        }

        private void CreateItem(WeatherClient weather)
        {
            var scenario = feature.Scenario("Create an item");

            weather.Create(new TodoItem {Id = 123, Name = "do sth"})
                   .InStep(scenario.Step("Create TODO item"))
                   .ShouldBe(EqualToPattern("/Patterns/Create.json")
                                .Expected("Item is created and its details are returned"))
                   .ShouldBe(InStatus(HttpStatusCode.Created));
        }

        private CompareOperationWithPattern EqualToPattern([PathReference] string file)
            => new CompareOperationWithPattern(Path + file);

        private VerifyResponseStatus InStatus(HttpStatusCode status)
            => new VerifyResponseStatus(status);
    }
}