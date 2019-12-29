using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Synergy.Samples.Web.API.Tests 
{
    public class SwaggerClient 
    {
        private readonly TestServer testServer;
        private HttpClient httpClient => testServer.HttpClient;

        public SwaggerClient(TestServer testServer)
        {
            this.testServer = testServer;
        }

        public async Task validate_api_contract_changes(string pathToContract, string version = "v1")
        {
            // ACT
            var newContract = await GetCurrentSwaggerContract(version);

            // ASSERT
            var patternContract = ReadSwaggerContractFromSavedPattern(pathToContract);
            var contracts = new JsonComparer(patternContract, newContract, new Ignore("info.description"));

            if (contracts.AreEquivalent == false)
            {
                File.WriteAllText(pathToContract, newContract.ToString(Formatting.Indented));
            }

            Assert.IsTrue(contracts.AreEquivalent, $"Swagger API description was modified. Verify the differences:\n\n {contracts.GetDifferences()}");
        }

        private async Task<JObject> GetCurrentSwaggerContract([NotNull] string version)
        {
            var url = testServer.PrepareRequestUri($"/swagger/{version}/swagger.json");
            var response = httpClient.GetAsync(url);
            var contract = await response.Result.Content.ReadAsStringAsync();
            return JObject.Parse(contract);
        }

        private static JObject ReadSwaggerContractFromSavedPattern(string pathToContract)
        {
            if (File.Exists(pathToContract) == false)
                return JObject.Parse("{}");

            return JObject.Parse(File.ReadAllText(pathToContract));
        }
    }
}