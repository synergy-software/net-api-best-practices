using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using Synergy.Contracts;
using Synergy.Web.Api.Testing.Assertions;
using Newtonsoft.Json.Linq;

namespace Synergy.Samples.Web.API.Tests.Infrastructure
{
    public static class ApiConventionFor
    {
        private static IAssertion RequestMethodIs(HttpMethod method)
        {
            return new VerifyRequestMethod(method)
               .Expected("Convention: HTTP request method is GET");
        }

        private static IAssertion ResponseStatusIs(HttpStatusCode status)
        {
            return new VerifyResponseStatus(status)
               .Expected($"Convention: Returned HTTP status code is {(int)status} ({status})");
        }

        private static IEnumerable<IAssertion> CreationRequest()
        {
            yield return RequestMethodIs(HttpMethod.Post);
        }

        /// <summary>
        /// Gets set of assertions that verify if creation operation meets convention.
        /// E.g. if created element is returned properly from Web API or if POST method is used, etc.
        /// </summary>
        public static IEnumerable<IAssertion> Create()
        {
            // Request
            foreach (var assertion in CreationRequest()) 
                yield return assertion;

            // Response
            yield return ResponseStatusIs(HttpStatusCode.Created);

            yield return new VerifyResponseHeader("Location",
                                                  value => Fail.IfWhitespace(value, Violation.Of("There is no 'Location' header returned")))
               .Expected("Convention: Location header (pointing to newly created element) is returned with response.");

            yield return ResponseContentTypeIsJson();
        }

        public static IEnumerable<IAssertion> GettingList()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.OK);
            yield return ResponseContentTypeIsJson();
        }

        private static IAssertion ResponseContentTypeIsJson()
        {
            return new VerifyResponseContentType(MediaTypeNames.Application.Json)
               .Expected($"Convention: Returned HTTP Content-Type is \"{MediaTypeNames.Application.Json}\"");
        }

        public static IEnumerable<IAssertion> CreateWithValidationError()
        {
            return CreationRequest()
               .Concat(BadRequest());

            // TODO: Add response validation - it should NOT contain "Location" header
        }

        private static IEnumerable<IAssertion> BadRequest()
        {
            yield return ResponseStatusIs(HttpStatusCode.BadRequest);

            yield return new VerifyResponseBody("message", token => ValidateIfNodeExists(token, "message"))
               .Expected("Convention: error JSON contains \"message\" node");

            yield return new VerifyResponseBody("traceId", token => ValidateIfNodeExists(token, "traceId"))
               .Expected("Convention: error JSON contains \"traceId\" node");
        }

        private static void ValidateIfNodeExists(JToken? token, string node)
        {
            token.FailIfNull(Violation.Of($"\"{node}\" is not present"));
            var value = token.Value<string>();
            Fail.IfWhitespace(value, Violation.Of($"\"{node}\" is empty"));
        }

        public static IEnumerable<IAssertion> Http404NotFound()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.NotFound);
        }

        public static IEnumerable<IAssertion> CreatedResourcePointedByLocation()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.OK);
            yield return ResponseContentTypeIsJson();
        }

        public static IEnumerable<IAssertion> GetSingleResource()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.OK);
            yield return ResponseContentTypeIsJson();
        }
    }
}