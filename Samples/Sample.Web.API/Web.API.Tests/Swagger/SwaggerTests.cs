using NUnit.Framework;
using Synergy.Samples.Web.API.Tests.Swagger;

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
            var testServer = new TestServer();
            var swagger = new SwaggerClient(testServer);

            // ACT
            swagger.GetSwaggerContract(version)
                   .ShouldBe(new CompareContractWithPattern(Path + $"swagger-{version}.json", new Ignore("info.description")));
        }
    }
}
