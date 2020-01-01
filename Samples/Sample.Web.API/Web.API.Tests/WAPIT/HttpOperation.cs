﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Tests.WAPIT.Assertions;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public class HttpOperation
    {
        public string? Description { get; private set; }
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

        // TODO: Dodaj możliwość asertowania konwencji - np. standardowy wygląd operacji CREATE

        public HttpOperation ShouldBe(IAssertion assertion)
        {
            Assertions.Add(assertion);
            assertion.Assert(this);
            return this;
        }

        public HttpOperation ShouldBe(IEnumerable<IAssertion> assertions)
        {
            foreach (var assertion in assertions)
            {
                this.ShouldBe(assertion);
            }

            return this;
        }

        public HttpOperation Details(string details)
        {
            this.Description = details.OrFailIfWhiteSpace(nameof(details));
            return this;
        }
    }
}