using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
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
            CreateUser(users);
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

        private string CreateUser(UsersClient users)
        {
            var scenario = feature.Scenario("Create a user");

            users.Create("marcin@synergy.com")
                 .InStep(scenario.Step("Create new user"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/Create.json")
                         .Ignore(ResponseBody("user.id"))
                         .Expected("User is created and its details are returned"))
                 .ShouldBe(ApiConventionFor.Create())
                 .Response.Content.Read("user.id", out string id);

            return id;
        }

        private void TryToCreateItemWithEmptyName(UsersClient users)
        {
            var scenario = feature.Scenario("Try to create user without login");

#pragma warning disable CS8625
            users.Create(null)
                   .InStep(scenario.Step("Create user with a null login"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateNullLogin.json")
                            .Ignore(ignoreError)
                            .Expected("User is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());
#pragma warning restore CS8625

            users.Create("")
                   .InStep(scenario.Step("Create user with an empty login"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateEmptyLogin.json")
                            .Ignore(ignoreError)
                            .Expected("User is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());

            users.Create("  " )
                   .InStep(scenario.Step("Create user item with a whitespace login"))
                   .ShouldBe(EqualToPattern("/Patterns/TryToCreateWhitespaceLogin.json")
                            .Ignore(ignoreError)
                            .Expected("User is NOT created and error is returned"))
                   .ShouldBe(ApiConventionFor.CreateWithValidationError());
        }

        private CompareOperationWithPattern EqualToPattern([PathReference] string file)
            => new CompareOperationWithPattern(Path + file);

        private static VerifyResponseStatus InStatus(HttpStatusCode expected) 
            => new VerifyResponseStatus(expected);
    }
}