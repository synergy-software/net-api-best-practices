﻿using System.Net;
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
    public class UserScenario
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
            GetEmptyListOfUsers();
            var userId = CreateUser();
            GetUser(userId);
            GetListOfUsers();
            TryToCreateUserWithErrors();
            DeleteUser(userId);

            new Markdown(feature).GenerateReportTo(Path + "/Users.md");
            testServer.FailIfLeftInRepairMode();
        }

        private void GetEmptyListOfUsers()
        {
            var scenario = feature.Scenario("Get empty list of users");

            users.GetAll()
                 .InStep(scenario.Step("Retrieve users"))
                 .ShouldBe(ApiConventionFor.GetListOfResources())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S01_E01_GetEmptyListOfUsers.json")
                         .Expected("Manual: Empty users list is returned")
                      );
        }

        private string CreateUser()
        {
            var scenario = feature.Scenario("Create a user");

            users.Create("marcin@synergy.com")
                 .InStep(scenario.Step("Create new user"))
                 .ShouldBe(ApiConventionFor.CreateResource())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_E01_CreateUser.json")
                         .Ignore(ResponseLocationHeader())
                         .Ignore(ResponseBody("user.id"))
                         .Expected("Manual: User is created and its details are returned"))
                 .ReadUserId(out var id)
                 .ReadCreatedUserLocationUrl(out var location);

            users.GetUserBy(location)
                 .InStep(scenario.Step("Get created user pointed by \"Location\" header"))
                 .ShouldBe(ApiConventionFor.GetSingleResource())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_E02_GetCreatedUserByLocation.json")
                         .Ignore(RequestMethod())
                         .Ignore(ResponseBody("user.id"))
                         .Expected("Manual: User details are returned"));

            return id;
        }

        private void GetUser(string userId)
        {
            var scenario = feature.Scenario("Get user");

            users.GetUser(userId)
                 .InStep(scenario.Step("Get user by id"))
                 .ShouldBe(ApiConventionFor.GetSingleResource())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S03_E01_GetUser.json")
                         .Ignore(RequestMethod())
                         .Ignore(ResponseBody("user.id"))
                         .Expected("Manual: User details are returned"));

            users.GetUser("user-id-that-do-not-exist")
                 .InStep(scenario.Step("Negative test: Try to get user that do not exist"))
                 .ShouldBe(ApiConventionFor.GetSingleResourceThatDoNotExist())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S03_N02_GetUserThatDoNotExist.json")
                         .Ignore(RequestMethod())
                         .Ignore(errorNodes)
                         .Expected("Manual: No user details are returned and 404 error (not found) is returned instead"));
        }

        private void GetListOfUsers()
        {
            var scenario = feature.Scenario("Get list of users");

            users.GetAll()
                 .InStep(scenario.Step("Retrieve users"))
                 .ShouldBe(ApiConventionFor.GetListOfResources())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S04_E01_GetListOfUsers.json")
                         .Ignore(ResponseBody("users[*].id"))
                         .Expected("Manual: Users list is returned")
                      );
        }

        private void DeleteUser(string userId)
        {
            var scenario = feature.Scenario("Delete user");

            users.DeleteUser(userId)
                 .InStep(scenario.Step("Delete user by id"))
                 .ShouldBe(ApiConventionFor.DeleteResource())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S05_E01_DeleteUser.json")
                         .Ignore(RequestMethod())
                         .Expected("Manual: User is deleted and no details are returned"));

            users.GetUser(userId)
                 .InStep(scenario.Step("Try to get the deleted user"))
                 .ShouldBe(ApiConventionFor.GetSingleResourceThatDoNotExist())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S05_E02_GetDeletedUser.json")
                         .Ignore(RequestMethod())
                         .Ignore(errorNodes.And(ResponseBody("message")))
                         .Expected("Manual: User is not found and error is returned"));
        }

        private void TryToCreateUserWithErrors()
        {
            var scenario = feature.Scenario("Try to create user without login");

#pragma warning disable CS8625
            users.Create(null)
                 .InStep(scenario.Step("Negative test: Create user with a null login"))
                 .ShouldBe(ApiConventionFor.CreateResourceWithValidationError())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_N01_TryToCreateUserWithNullLogin.json")
                         .Ignore(errorNodes)
                         .Expected("Manual: User is NOT created and error is returned"));
#pragma warning restore CS8625

            users.Create("")
                 .InStep(scenario.Step("Negative test: Create user with an empty login"))
                 .ShouldBe(ApiConventionFor.CreateResourceWithValidationError())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_N02_TryToCreateUserWithEmptyLogin.json")
                         .Ignore(errorNodes)
                         .Expected("Manual: User is NOT created and error is returned"));

            users.Create("  ")
                 .InStep(scenario.Step("Negative test: Create user with a whitespace login"))
                 .ShouldBe(ApiConventionFor.CreateResourceWithValidationError())
                 .ShouldBe(
                      EqualToPattern("/Patterns/S02_N03_TryToCreateUserWithWhitespaceLogin.json")
                         .Ignore(errorNodes)
                         .Expected("Manual: User is NOT created and error is returned"));
        }

        private CompareOperationWithPattern EqualToPattern([PathReference] string file)
            => new CompareOperationWithPattern(Path + file);

        private static VerifyResponseStatus InStatus(HttpStatusCode expected)
            => new VerifyResponseStatus(expected);
    }
}