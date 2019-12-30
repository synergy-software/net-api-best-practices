using NUnit.Framework;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Samples.Web.API.Tests.Swagger;
using Synergy.Samples.Web.API.Tests.WAPIT.Assertions;

namespace Synergy.Samples.Web.API.Tests
{
    [TestFixture]
    public class SwaggerTests
    {
        private const string Path = @"../../../Contracts/";

        [Test]
        [TestCase("v1")]
        public void validate_api_contract_changes(string version)
        {
            // ARRANGE
            var testServer = new SampleTestServer();
            var swagger = new SwaggerClient(testServer);

            // ACT
            swagger.GetSwaggerContract(version)
                   .ShouldBe(EqualToPatternIn(version));
        }

        private static CompareResponseWithPattern EqualToPatternIn(string version)
        {
            return new CompareResponseWithPattern(
                                                  Path + $"swagger-{version}.json",
                                                  new Ignore("info.description"),
                                                  CompareResponseWithPattern.Mode.ContractCheck
                                                 );
        }
    }
}
