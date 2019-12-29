using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Sample.Web;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Extensions;
using Synergy.Samples.Web.API.Tests.WAPIT;

namespace Synergy.Samples.Web.API.Tests
{
    public class TestServer
    {
        private HttpClient HttpClient { get; }
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
            var request = CreateHttpRequest(HttpMethod.Get, path, queryParameters);
            return Send(request);
        }

        public HttpOperation Post(string path, [CanBeNull] object? urlParameters = null, object? content = null)
        {
            var request = CreateHttpRequest(HttpMethod.Post, path, urlParameters, content);
            return Send(request);
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod httpMethod, string path, object? queryParameters, object? content = null)
        {
            var request = new HttpRequestMessage
                                     {
                                         Method = httpMethod,
                                         RequestUri = this.PrepareRequestUri(path, queryParameters)
                                     };
            if (content != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            return request;
        }

        private HttpOperation Send(HttpRequestMessage request)
        {
            Stopwatch timer = Stopwatch.StartNew();
            var response = this.HttpClient.SendAsync(request).Result;
            timer.Stop();

            return new HttpOperation(this, request, response, timer);
        }
    }
}