using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Sample.API.Controllers;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Samples.Web.API.Tests.Weather;
using Synergy.Web.Api.Testing;
using Synergy.Web.Api.Testing.Assertions;
using Synergy.Web.Api.Testing.Features;
using Synergy.Web.Api.Testing.Json;
using static Synergy.Web.Api.Testing.Json.Ignore;

namespace Synergy.Samples.Web.API.Tests.Users
{
    [TestFixture]
    public class UsersScenario
    {
        private const string Path = @"../../../Users";
        private readonly Feature feature = new Feature("Manage users through API");
        private readonly Ignore ignoreError = ResponseBody("traceId");

        [Test]
        public void get_weather()
        {
            // ARRANGE
            var testServer = new SampleTestServer();
            var users = new UsersClient(testServer);
            testServer.Repair = false;

            // SCENARIO
            GetUsers(users);
            CreateItem(users);
            TryToCreateItemWithEmptyName(users);

            if (testServer.Repair)
            {
                new Markdown(feature).GenerateReportTo(Path + "/Users.md");
                Assert.IsFalse(testServer.Repair, "Test server is in repair mode. Do not leave it like that.");
            }
        }

        private void GetUsers(UsersClient users)
        {
            var scenario = feature.Scenario("Get users forecast");

            users.GetAll()
                 .InStep(scenario.Step("Retrieve users forecast"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/GetUsers.json")
                         .Expected("Empty users list is returned")
                      )
                 .ShouldBe(ApiConventionFor.GettingList());
        }

        private int CreateItem(UsersClient users)
        {
            var scenario = feature.Scenario("Create an item");

            users.Create(new TodoItem {Id = 123, Name = "do sth"})
                   .InStep(scenario.Step("Create TODO item"))
                   .ShouldBe(EqualToPattern("/Patterns/Create.json")
                                .Expected("Item is created and its details are returned"))
                   .ShouldBe(ApiConventionFor.Create())
                   .Response.Content.Read("id", out int id);

            return id;
        }

        private void TryToCreateItemWithEmptyName(UsersClient users)
        {
            var scenario = feature.Scenario("Try to create an item without a name");

            #pragma warning disable CS8625
            users.Create(new TodoItem {Id = 123, Name = null})
                   .InStep(scenario.Step("Create TODO item with a null name"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateNullName.json")
                            .Ignore(ignoreError)
                            .Expected("Item is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());
            #pragma warning restore CS8625

            users.Create(new TodoItem {Id = 123, Name = ""})
                   .InStep(scenario.Step("Create TODO item with an empty name"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateEmptyName.json")
                            .Ignore(ignoreError)
                            .Expected("Item is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());

            users.Create(new TodoItem {Id = 123, Name = "  "})
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