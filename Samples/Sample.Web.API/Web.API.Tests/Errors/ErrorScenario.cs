using System.Net;
using JetBrains.Annotations;
using NUnit.Framework;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Web.Api.Testing;
using Synergy.Web.Api.Testing.Assertions;
using Synergy.Web.Api.Testing.Features;

namespace Synergy.Samples.Web.API.Tests.Errors
{
    [TestFixture]
    public class ErrorScenario
    {
        private const string Path = @"../../../Errors";
        private readonly Feature feature = new Feature("Check API errors");

        [Test]
        public void check_errors()
        {
            // ARRANGE
            var testServer = new SampleTestServer();
            var errors = new ErrorsClient(testServer);
            testServer.Repair = false;

            // SCENARIO
            Get404NotFound(errors);

            new Markdown(feature).GenerateReportTo(Path + "/Errors.md");
            Assert.IsFalse(testServer.Repair, "Test server is in repair mode. Do not leave it like that.");
        }

        private void Get404NotFound(ErrorsClient users)
        {
            var scenario = feature.Scenario("Get not existing resource from API");

            users.GetNonExistingApi()
                 .InStep(scenario.Step("Try to retrieve not existing resource"))
                 .ShouldBe(
                      EqualToPattern("/Patterns/Get404.json")
                         .Expected("Error is returned")
                      )
                 .ShouldBe(ApiConventionFor.Http404NotFound());
        }

        private CompareOperationWithPattern EqualToPattern([PathReference] string file)
            => new CompareOperationWithPattern(Path + file);

        private static VerifyResponseStatus InStatus(HttpStatusCode expected) 
            => new VerifyResponseStatus(expected);
    }
}