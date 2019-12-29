using System.Diagnostics.CodeAnalysis;
using Synergy.Samples.Web.API.Tests.WAPIT;

namespace Synergy.Samples.Web.API.Tests.Swagger
{
    public class SwaggerClient
    {
        private readonly TestServer _testServer;

        public SwaggerClient(TestServer testServer)
        {
            _testServer = testServer;
        }

        public HttpOperation GetSwaggerContract([NotNull] string version) 
            => _testServer.Get($"/swagger/{version}/swagger.json");
    }
}