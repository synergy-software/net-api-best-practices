using System;
using System.Net.Http;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sample.Web;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Extensions;
using Synergy.Samples.Web.API.Tests.WAPIT;

namespace Synergy.Samples.Web.API.Tests
{
    public class TestServer
    {
        public HttpClient HttpClient { get; }
        public bool Repair { get; set; }

        public TestServer()
        {
            var applicationFactory = StartupTestServerUsing<Startup>();
            HttpClient = applicationFactory.CreateClient();
        }

        private WebApplicationFactory<TStartup> StartupTestServerUsing<TStartup>() where TStartup : class
        {
            return new WebApplicationFactory<TStartup>()
                .WithWebHostBuilder(configuration =>
                {
                    configuration.UseEnvironment(Application.Environment.Tests);
                });
        }

        public Uri PrepareRequestUri(string path, [CanBeNull] object? parameterToGet = null)
        {
            Fail.IfWhitespace(path, nameof(path));

            var uriBuilder = new UriBuilder
            {
                Path = path,
                Host = HttpClient.BaseAddress.Host,
                Port = HttpClient.BaseAddress.Port
            };

            if (parameterToGet != null)
                uriBuilder.Query = QueryBuilder.Build(parameterToGet);

            return uriBuilder.Uri;
        }

        public HttpOperation Get(string path, [CanBeNull] object? queryParameters = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = this.PrepareRequestUri(path,  queryParameters)
            };

            var response = this.HttpClient.SendAsync(request).Result;

            return new HttpOperation(this, request, response);
        }
    }
}