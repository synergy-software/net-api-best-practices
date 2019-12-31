﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Assertions 
{
    public class CompareOperationWithPattern : IAssertion
    {
        private readonly string _patternFilePath;
        private readonly Ignore _ignore;
        private JToken? _savedPattern;
        public string? ExpectedResult {get; private set; }

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

        public void Assert(HttpOperation operation)
        {
            var current = GeneratePattern(operation);
            if (_savedPattern == null)
            {
                SaveNewPattern(current);
                return;
            }

            JsonComparer patterns = new JsonComparer(_savedPattern, current, _ignore);

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
            yield return new JProperty("method", request.GetRequestedUrl());

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

            yield return new JProperty("status", $"{(int)response.StatusCode} {response.ReasonPhrase}");

            var headers = response.Headers.Select(GetHeader);
            yield return new JProperty("headers", new JObject(headers));

            var responseJson = response.Content.ReadJson();
            yield return new JProperty("body", responseJson);
        }

        private static JProperty GetHeader(KeyValuePair<string, IEnumerable<string>> header)
        {
            return new JProperty(header.Key, String.Join("; ", header.Value));
        }

        public CompareOperationWithPattern Ignore(params string[] ignores)
        {
            this._ignore.Append(ignores);
            return this;
        }

        public CompareOperationWithPattern Ignore(Ignore ignore)
        {
            this._ignore.Append(ignore.Nodes);
            return this;
        }

        public IAssertion Expected(string expected)
        {
            this.ExpectedResult = expected.OrFailIfWhiteSpace(nameof(expected));
            return this;
        }
    }
}