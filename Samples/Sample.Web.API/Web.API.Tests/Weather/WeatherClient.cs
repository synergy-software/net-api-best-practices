using Synergy.Samples.Web.API.Tests.WAPIT;

namespace Synergy.Samples.Web.API.Tests.Weather
{
    public class WeatherClient
    {
        private readonly TestServer _testServer;

        public WeatherClient(TestServer testServer)
        {
            _testServer = testServer;
        }

        public HttpOperation GetWeatherForecast() => _testServer.Get("api/v1/weather/forecast");
    }
}