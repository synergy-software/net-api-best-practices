using System.Threading.Tasks;
using NUnit.Framework;

namespace Synergy.Samples.Web.API.Tests
{
    [TestFixture]
    public class SwaggerTests
    {
        private const string Path = @"../../../Contracts/";

        [Test]
        [TestCase("v1")]
        public async Task validate_api_contract_changes(string version)
        {
            // ARRANGE
            var testServer = new TestServer();
            var swagger = new SwaggerClient(testServer);

            // ACT
            await swagger.validate_api_contract_changes(Path + $"swagger-{version}.json", version);
        }
    }
}
