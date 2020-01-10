using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Synergy.Contracts;
using Synergy.Web.Api.Testing.Assertions;

namespace Synergy.Web.Api.Testing
{
    public class HttpOperation
    {
        public string? Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public TestServer TestServer { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public HttpResponseMessage Response { get; private set; }
        public readonly List<IAssertion> Assertions = new List<IAssertion>();

        public void Init(TestServer testServer, HttpRequestMessage request, HttpResponseMessage response, Stopwatch timer)
        {
            Duration = timer.Elapsed;
            TestServer = testServer.OrFail(nameof(testServer));
            Request = request.OrFail(nameof(request));
            Response = response.OrFail(nameof(response));
        }

        internal void Assert(IEnumerable<IAssertion> assertions)
        {
            foreach (var assertion in assertions)
            {
                Assertions.Add(assertion);
                assertion.Assert(this);
            }
        }

        internal void SetDescription(string details)
        {
            Description = details.OrFailIfWhiteSpace(nameof(details));
        }
    }
}