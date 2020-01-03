using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;
using Synergy.Web.Api.Testing.Features;
using Synergy.Web.Api.Testing.Json;

namespace Synergy.Web.Api.Testing.Assertions
{
    public class CompareOperationWithPattern : Assertion, IHttpRequestStorage, IHttpResponseStorage
    {
        private readonly string _patternFilePath;
        private readonly Ignore _ignore;
        private JToken? _savedPattern;

        public CompareOperationWithPattern(string patternFilePath, Ignore? ignore = null)
        {
            _patternFilePath = patternFilePath;
            _ignore = ignore ?? new Ignore();
            if (File.Exists(patternFilePath))
            {
                var content = File.ReadAllText(patternFilePath);
                _savedPattern = JObject.Parse(content);
            }
        }

        public override void Assert(HttpOperation operation)
        {
            var current = GeneratePattern(operation);
            if (_savedPattern == null)
            {
                SaveNewPattern(current);
                return;
            }

            var patterns = new JsonComparer(_savedPattern, current, _ignore);

            if (operation.TestServer.Repair && patterns.AreEquivalent == false)
            {
                SaveNewPattern(current);
                return;
            }

            Fail.IfFalse(patterns.AreEquivalent,
                         Violation.Of("Operation is different than expected. Verify the differences:\n\n {0}", patterns.GetDifferences())
                        );
        }

        private void SaveNewPattern(JObject current)
        {
            _savedPattern = current;
            File.WriteAllText(_patternFilePath, current.ToString(Formatting.Indented));
        }

        private static JObject GeneratePattern(HttpOperation operation)
        {
            return new JObject(
                               new JProperty("request", new JObject(GetRequestProperties(operation))),
                               new JProperty("response", new JObject(GetResponseProperties(operation)))
                              );
        }

        private static IEnumerable<JProperty> GetRequestProperties(HttpOperation operation)
        {
            var request = operation.Request;
            yield return new JProperty("method", request.GetRequestFullMethod());

            var requestJson = request.Content.ReadJson();
            if (requestJson != null)
            {
                yield return new JProperty("body", requestJson);
            }

            var headers = request.Headers.Select(GetHeader);
            yield return new JProperty("headers", new JObject(headers));
        }

        private static IEnumerable<JProperty> GetResponseProperties(HttpOperation operation)
        {
            var response = operation.Response;

            yield return new JProperty("status", $"{(int) response.StatusCode} {response.ReasonPhrase}");

            var headers = response.Headers.Select(GetHeader);
            yield return new JProperty("headers", new JObject(headers));

            var responseJson = response.Content.ReadJson();
            yield return new JProperty("body", responseJson);
        }

        private static JProperty GetHeader(KeyValuePair<string, IEnumerable<string>> header)
        {
            return new JProperty(header.Key, string.Join("; ", header.Value));
        }

        public CompareOperationWithPattern Ignore(params string[] ignores)
        {
            _ignore.Append(ignores);
            return this;
        }

        public CompareOperationWithPattern Ignore(Ignore ignore)
        {
            _ignore.Append(ignore.Nodes);
            return this;
        }

        HttpRequestMessage IHttpRequestStorage.GetSavedRequest()
        {
            Fail.IfNull(_savedPattern, nameof(_savedPattern));

            var fullMethod = _savedPattern!.SelectToken("$.request.method").Value<string>();
            var method = fullMethod.Substring(0, fullMethod.IndexOf(" "));
            var url = fullMethod.Substring(fullMethod.IndexOf(" "));
            var request = new HttpRequestMessage(new HttpMethod(method), url);

            var body = _savedPattern!.SelectToken("$.request.body");
            if (body != null)
            {
                var payload = body.ToString();
                request.Content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);
            }

            return request;
        }

        HttpResponseMessage IHttpResponseStorage.GetSavedResponse()
        {
            Fail.IfNull(_savedPattern, nameof(_savedPattern));

            var fullStatus = _savedPattern!.SelectToken("$.response.status").Value<string>();
            var status = fullStatus.Substring(0, fullStatus.IndexOf(" "));
            var statusCode = Enum.Parse<HttpStatusCode>(status);
            var response = new HttpResponseMessage(statusCode);
            var body = _savedPattern!.SelectToken("$.response.body").ToString();
            response.Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);
            var headers = _savedPattern!.SelectTokens("$.response.headers.*");
            foreach (var header in headers)
            {
                response.Headers.Add(header.Path.Replace("response.headers.", ""), header.Value<string>());
            }

            return response;
        }
    }
}