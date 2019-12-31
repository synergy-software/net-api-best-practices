using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Sample.API.Controllers;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Samples.Web.API.Tests.WAPIT.Assertions;

namespace Synergy.Samples.Web.API.Tests.Weather
{
    [TestFixture]
    public class WeatherTests
    {
        private const string Path = @"../../../Weather";

        [Test]
        public void get_weather()
        {
            // ARRANGE
            var testServer = new SampleTestServer();
            var weather = new WeatherClient(testServer);

            testServer.Repair = false;

            // SCENARIO
            weather.GetWeatherForecast()
                   .ShouldBe(EqualToPattern("/Patterns/GetWeatherForecast.json")
                            .Ignore("$.response.body")
                            .Expected("Weather forecast is returned"))
                   .ShouldBe(InStatus(HttpStatusCode.OK));

            weather.Create(new TodoItem {Id = 123, Name = "do sth"})
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