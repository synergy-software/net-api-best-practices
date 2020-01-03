using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Sample.API.Controllers;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Web.Api.Testing;
using Synergy.Web.Api.Testing.Assertions;
using Synergy.Web.Api.Testing.Features;
using Synergy.Web.Api.Testing.Json;
using static Synergy.Web.Api.Testing.Json.Ignore;

namespace Synergy.Samples.Web.API.Tests.Weather
{
    [TestFixture]
    public class WeatherTests
    {
        private const string Path = @"../../../Weather";
        private readonly Feature feature = new Feature("Manage weather through API");
        private readonly Ignore ignoreError = ResponseBody("traceId");

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
            TryToCreateItemWithEmptyName(weather);

            if (testServer.Repair)
            {
                new Markdown(feature).GenerateReportTo(Path + "/Weather.md");
            }

            Assert.IsFalse(testServer.Repair, "Test server is in repair mode. Do not leave it like that.");
        }

        private void GetWeatherForecast(WeatherClient weather)
        {
            var scenario = feature.Scenario("Get weather forecast");

            weather.GetWeatherForecast()
                   .InStep(scenario.Step("Retrieve weather forecast"))
                   .ShouldBe(EqualToPattern("/Patterns/GetWeatherForecast.json")
                            .Ignore(ResponseBody())
                            .Expected("Weather forecast is returned"))
                   .ShouldBe(ApiConventionFor.GettingList());
        }

        private int CreateItem(WeatherClient weather)
        {
            var scenario = feature.Scenario("Create an item");

            weather.Create(new TodoItem {Id = 123, Name = "do sth"})
                   .InStep(scenario.Step("Create TODO item"))
                   .ShouldBe(EqualToPattern("/Patterns/Create.json")
                                .Expected("Item is created and its details are returned"))
                   .ShouldBe(ApiConventionFor.Create())
                   .Response.Content.Read("id", out int id);

            return id;
        }

        private void TryToCreateItemWithEmptyName(WeatherClient weather)
        {
            var scenario = feature.Scenario("Try to create an item without a name");

            #pragma warning disable CS8625
            weather.Create(new TodoItem {Id = 123, Name = null})
                   .InStep(scenario.Step("Create TODO item with a null name"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateNullName.json")
                            .Ignore(ignoreError)
                            .Expected("Item is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());
            #pragma warning restore CS8625

            weather.Create(new TodoItem {Id = 123, Name = ""})
                   .InStep(scenario.Step("Create TODO item with an empty name"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateEmptyName.json")
                            .Ignore(ignoreError)
                            .Expected("Item is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());

            weather.Create(new TodoItem {Id = 123, Name = "  "})
                   .InStep(scenario.Step("Create TODO item with an whitespace name"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateWhitespaceName.json")
                            .Ignore(ignoreError)
                            .Expected("Item is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());
        }

        private CompareOperationWithPattern EqualToPattern([PathReference] string file)
            => new CompareOperationWithPattern(Path + file);

        private static VerifyResponseStatus InStatus(HttpStatusCode expected) 
            => new VerifyResponseStatus(expected);
    }
}