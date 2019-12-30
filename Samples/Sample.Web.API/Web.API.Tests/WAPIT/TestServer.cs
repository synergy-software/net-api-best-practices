using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Tests.WAPIT;

namespace Synergy.Samples.Web.API.Tests
{
    public abstract class TestServer
    {
        public HttpClient HttpClient { get; }
        public bool Repair { get; set; }

        protected TestServer()
        {
            HttpClient = Start();
        }

        protected abstract HttpClient Start();

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

        public HttpOperation Get(string path, [CanBeNull] object? urlParameters = null)
        {
            var request = CreateHttpRequest(HttpMethod.Get, path, urlParameters);
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