using System.Collections.Generic;
using System.Net.Http;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public class HttpOperation
    {
        public readonly TestServer TestServer;
        public readonly HttpRequestMessage Request;
        public readonly HttpResponseMessage Response;
        public readonly List<IPattern> Patterns = new List<IPattern>(3);

        public HttpOperation(TestServer testServer, HttpRequestMessage request, HttpResponseMessage response)
        {
            TestServer = testServer.OrFail(nameof(testServer));
            Request = request.OrFail(nameof(request));
            Response = response.OrFail(nameof(response));
        }

        public HttpOperation ShouldBe(IPattern pattern)
        {
            Patterns.Add(pattern);
            pattern.Equals(this);
            return this;
        }
    }
}