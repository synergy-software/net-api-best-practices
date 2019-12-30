using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Tests.WAPIT.Assertions;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public class HttpOperation
    {
        public TimeSpan Duration { get; }
        public readonly TestServer TestServer;
        public readonly HttpRequestMessage Request;
        public readonly HttpResponseMessage Response;
        public readonly List<IAssertion> Assertions = new List<IAssertion>();

        public HttpOperation(TestServer testServer, HttpRequestMessage request, HttpResponseMessage response, Stopwatch timer)
        {
            Duration = timer.Elapsed;
            TestServer = testServer.OrFail(nameof(testServer));
            Request = request.OrFail(nameof(request));
            Response = response.OrFail(nameof(response));
        }

        public HttpOperation ShouldBe(IAssertion assertion)
        {
            Assertions.Add(assertion);
            assertion.Assert(this);
            return this;
        }
    }
}