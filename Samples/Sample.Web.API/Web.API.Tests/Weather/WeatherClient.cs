using Sample.API.Controllers;
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

        public HttpOperation GetWeatherForecast()
            => _testServer.Get("api/v1/weather/forecast")
                          .Details("Get weather forecast");

        public HttpOperation Create(TodoItem todo)
            => _testServer.Post("api/v1/weather", body: todo)
                          .Details($"Create a new TODO item named '{todo.Name}'");
    }
}