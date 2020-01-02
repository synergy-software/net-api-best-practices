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

        public HttpOperation Get(string path, [CanBeNull] object? urlParameters = null)
            => Send(HttpMethod.Get, path, urlParameters);

        public HttpOperation Post(string path, [CanBeNull] object? urlParameters = null, object? body = null)
            => Send(HttpMethod.Post, path, urlParameters, body);

        public HttpOperation Put(string path, [CanBeNull] object? urlParameters = null, object? body = null)
            => Send(HttpMethod.Put, path, urlParameters, body);

        public HttpOperation Patch(string path, [CanBeNull] object? urlParameters = null, object? body = null)
            => Send(HttpMethod.Patch, path, urlParameters, body);

        public HttpOperation Delete(string path, [CanBeNull] object? urlParameters = null)
            => Send(HttpMethod.Delete, path, urlParameters);

        private HttpOperation Send(HttpMethod httpMethod, string path, object? urlParameters, object? body = null)
        {
            var requestedOperation = CreateHttpRequest(httpMethod, path, urlParameters, body);

            var request = CreateHttpRequest(httpMethod, path, urlParameters, body);
            var timer = Stopwatch.StartNew();
            var response = HttpClient.SendAsync(request).Result;
            timer.Stop();

            return new HttpOperation(this, requestedOperation, response, timer);
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod httpMethod, string path, object? urlParameters, object? body = null)
        {
            var request = new HttpRequestMessage
                          {
                              Method = httpMethod,
                              RequestUri = PrepareRequestUri(path, urlParameters)
                          };

            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }

            return request;
        }

        private Uri PrepareRequestUri(string path, [CanBeNull] object? parameterToGet = null)
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
    }
}