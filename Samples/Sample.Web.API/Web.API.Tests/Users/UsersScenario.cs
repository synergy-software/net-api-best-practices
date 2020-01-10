using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Synergy.Samples.Web.API.Tests.Infrastructure;
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
        private SampleTestServer testServer;
        private UsersClient users;
        private const string Path = @"../../../Users";
        private readonly Feature feature = new Feature("Manage users through API");
        private readonly Ignore errorNodes = ResponseBody("traceId");

        [SetUp]
        public void Setup()
        {
            testServer = new SampleTestServer();
            users = new UsersClient(testServer);
            testServer.Repair = false;
        }

        [Test]
        public void manage_users_through_web_api()
        {
            // SCENARIO
            GetUsers();
            var userId = CreateUser();
            GetUser(userId);
            TryToCreateItemWithEmptyName();

            new Markdown(feature).GenerateReportTo(Path + "/Users.md");
            Assert.IsFalse(testServer.Repair, "Test server is in repair mode. Do not leave it like that.");
        }

        private void GetUsers()
        {
            var scenario = feature.Scenario("Get empty list of users");

            users.GetAll()
                 .InStep(scenario.Step("Retrieve users"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/S01_E01_GetEmptyListOfUsers.json")
                         .Expected("Manual: Empty users list is returned")
                      )
                 .ShouldBe(ApiConventionFor.GettingList());
        }

        private string CreateUser()
        {
            var scenario = feature.Scenario("Create a user");

            users.Create("marcin@synergy.com")
                 .InStep(scenario.Step("Create new user"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_E01_CreateUser.json")
                         .Ignore(ResponseLocationHeader())
                         .Ignore(ResponseBody("user.id"))
                         .Expected("Manual: User is created and its details are returned"))
                 .ShouldBe(ApiConventionFor.Create())
                 .ReadUserId(out var id)
                 .ReadCreatedUserLocation(out var location);

            users.GetUserBy(location)
                 .InStep(scenario.Step("Get created user pointed by \"Location\" header"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_E02_GetCreatedUserByLocation.json")
                         .Ignore(RequestMethod())
                         .Ignore(ResponseBody("id"))
                         .Expected("Manual: User details are returned"))
                 .ShouldBe(ApiConventionFor.CreatedResourcePointedByLocation());

            return id;
        }

        private void GetUser(string userId)
        {
            var scenario = feature.Scenario("Get user");

            users.GetUser(userId)
                 .InStep(scenario.Step("Get user by id"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/S03_E01_GetUser.json")
                         .Ignore(RequestMethod())
                         .Ignore(ResponseBody("id"))
                         .Expected("Manual: User details are returned"))
                 .ShouldBe(ApiConventionFor.GetSingleResource());
        }

        private void TryToCreateItemWithEmptyName()
        {
            var scenario = feature.Scenario("Try to create user without login");

#pragma warning disable CS8625
            users.Create(null)
                 .InStep(scenario.Step("Create user with a null login"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/TryToCreateNullLogin.json")
                         .Ignore(errorNodes)
                         .Expected("Manual: User is NOT created and error is returned"))
                 .ShouldBe(ApiConventionFor.CreateWithValidationError());
#pragma warning restore CS8625

            users.Create("")
                 .InStep(scenario.Step("Create user with an empty login"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/TryToCreateEmptyLogin.json")
                         .Ignore(errorNodes)
                         .Expected("Manual: User is NOT created and error is returned"))
                 .ShouldBe(ApiConventionFor.CreateWithValidationError());

            users.Create("  ")
                 .InStep(scenario.Step("Create user item with a whitespace login"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/TryToCreateWhitespaceLogin.json")
                         .Ignore(errorNodes)
                         .Expected("Manual: User is NOT created and error is returned"))
                 .ShouldBe(ApiConventionFor.CreateWithValidationError());
        }

        private CompareOperationWithPattern EqualToPattern([PathReference] string file)
            => new CompareOperationWithPattern(Path + file);

        private static VerifyResponseStatus InStatus(HttpStatusCode expected)
            => new VerifyResponseStatus(expected);
    }
}