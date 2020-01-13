using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using Synergy.Contracts;
using Synergy.Web.Api.Testing.Assertions;
using Newtonsoft.Json.Linq;
using Synergy.Web.Api.Testing;

namespace Synergy.Samples.Web.API.Tests.Infrastructure
{
    public static class ApiConventionFor
    {
        private static IAssertion RequestMethodIs(HttpMethod method)
        {
            return new VerifyRequestMethod(method)
               .Expected($"Convention: HTTP request method is {method}");
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
        public static IEnumerable<IAssertion> CreateResource()
        {
            // Request
            foreach (var assertion in CreationRequest()) 
                yield return assertion;

            // Response
            yield return ResponseStatusIs(HttpStatusCode.Created);

            yield return new VerifyResponseHeader(
                    "Location",
                    (operation, value)
                        =>
                    {
                        if (String.IsNullOrWhiteSpace(value) == false)
                            return Assertion.Ok;

                        return Assertion.Failure(
                            $"There is no 'Location' header returned in response:\n\n{operation.Response.ToHttpLook()}"
                            );
                    })
               .Expected("Convention: Location header (pointing to newly created element) is returned with response.");

            yield return ResponseContentTypeIs(MediaTypeNames.Application.Json);
        }

        public static IEnumerable<IAssertion> GetListOfResources()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.OK);
            yield return ResponseContentTypeIs(MediaTypeNames.Application.Json);
        }

        private static IAssertion ResponseContentTypeIs(string contentType)
        {
            return new VerifyResponseContentType(contentType)
               .Expected($"Convention: Returned HTTP Content-Type is \"{contentType}\"");
        }

        public static IEnumerable<IAssertion> CreateResourceWithValidationError()
        {
            foreach (var assertion in CreationRequest().Concat(BadRequest()))
            {
                yield return assertion;
            }

            yield return new VerifyResponseHeader(
                "Location",
                (operation, value)
                    =>
                {
                    if (String.IsNullOrWhiteSpace(value))
                        return Assertion.Ok;

                    return Assertion.Failure(
                        $"There is 'Location' header returned in response and it shouldn't be:\n\n{operation.Response.ToHttpLook()}"
                        );
                }).Expected("Convention: There is NO \"Location\" header returned in response");
        }

        private static IEnumerable<IAssertion> BadRequest()
        {
            yield return ResponseStatusIs(HttpStatusCode.BadRequest);

            yield return new VerifyResponseBody("message", (operation, token) => ValidateIfNodeExists(operation, token, "message"))
               .Expected("Convention: error JSON contains \"message\" node");

            yield return new VerifyResponseBody("traceId", (operation, token) => ValidateIfNodeExists(operation, token, "traceId"))
               .Expected("Convention: error JSON contains \"traceId\" node");
        }

        private static Assertion.Result ValidateIfNodeExists(HttpOperation operation, JToken? token, string node)
        {
            if (token == null)
                return Assertion.Failure($"\"{node}\" is not present in response: \n\n{operation.Response.ToHttpLook()}");

            var value = token.Value<string>();
            if (String.IsNullOrWhiteSpace(value))
            {
                return Assertion.Failure($"\"{node}\" is empty in response: \n\n{operation.Response.ToHttpLook()}");
            }

            return Assertion.Ok;

        }

        public static IEnumerable<IAssertion> Http404NotFound()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.NotFound);
        }

        public static IEnumerable<IAssertion> GetSingleResource()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.OK);
            yield return ResponseContentTypeIs(MediaTypeNames.Application.Json);
        }

        public static IEnumerable<IAssertion> GetSingleResourceThatDoNotExist()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Get);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.NotFound);
            yield return ResponseContentTypeIs(MediaTypeNames.Application.Json);
        }

        public static IEnumerable<IAssertion> DeleteResource()
        {
            // Request
            yield return RequestMethodIs(HttpMethod.Delete);

            // Response
            yield return ResponseStatusIs(HttpStatusCode.OK);
            yield return ResponseContentTypeIs(MediaTypeNames.Application.Json);
        }
    }
}