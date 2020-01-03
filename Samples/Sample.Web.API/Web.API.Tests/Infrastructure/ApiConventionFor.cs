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
        private static IEnumerable<IAssertion> CreationRequest()
        {
            yield return new VerifyRequestMethod(HttpMethod.Post)
               .Expected("Convention: HTTP request method is POST");
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
            yield return new VerifyResponseStatus(HttpStatusCode.Created)
               .Expected("Convention: Returned HTTP status code is 201 (Created)");

            yield return new VerifyResponseHeader("Location",
                                                  value => Fail.IfWhitespace(value, Violation.Of("There is no 'Location' header returned")))
               .Expected("Convention: Location header (pointing to newly created element) is returned with response.");

            yield return ResponseContentTypeIsJson();
        }

        public static IEnumerable<IAssertion> GettingList()
        {
            // Request
            yield return new VerifyRequestMethod(HttpMethod.Get)
               .Expected("Convention: HTTP request method is GET");

            // Response
            yield return new VerifyResponseStatus(HttpStatusCode.OK)
               .Expected("Convention: Returned HTTP status code is 200 (OK)");

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
        }

        private static IEnumerable<IAssertion> BadRequest()
        {
            yield return new VerifyResponseStatus(HttpStatusCode.BadRequest)
               .Expected("Convention: Returned HTTP status code is 400 (Bad Request)");

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
    }
}