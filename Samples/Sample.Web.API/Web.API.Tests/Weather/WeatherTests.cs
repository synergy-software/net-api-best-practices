using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Synergy.Samples.Web.API.Tests.WAPIT;

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
            var testServer = new TestServer();
            var weather = new WeatherClient(testServer);

            testServer.Repair = false;

            // ACT
            weather.GetWeatherForecast()
                   .ShouldBe(EqualToPattern("/Patterns/GetWeatherForecast.json").Ignore("$.response.content"))
                   .ShouldBe(InStatus(HttpStatusCode.OK));

            // TODO: Dodaj inne Should'y
        }

        private FullOperationPattern EqualToPattern([PathReference] string file)
            => new FullOperationPattern(Path + file);

        private ResponseStatusPattern InStatus(HttpStatusCode status)
            => new ResponseStatusPattern(status);
    }
}