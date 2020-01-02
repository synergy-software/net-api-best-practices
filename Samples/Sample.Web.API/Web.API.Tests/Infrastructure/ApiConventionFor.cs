using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Synergy.Contracts;
using Synergy.Web.Api.Testing.Assertions;

namespace Synergy.Samples.Web.API.Tests.Infrastructure
{
    public static class ApiConventionFor
    {
        /// <summary>
        /// Gets set of assertions that verify if creation operation meets convention.
        /// E.g. if created element is returned properly from Web API or if POST method is used, etc.
        /// </summary>
        public static IEnumerable<IAssertion> Create()
        {
            // Request
            yield return new VerifyRequestMethod(HttpMethod.Post)
               .Expected("Convention: HTTP request method is POST");

            // Response
            yield return new VerifyResponseStatus(HttpStatusCode.Created)
               .Expected("Convention: Returned HTTP status code is 201 (Created)");

            yield return new VerifyResponseHeader("Location", value => Fail.IfWhitespace(value, Violation.Of("There is no 'Location' header returned")))
               .Expected("Convention: Location header (pointing to newly created element) is returned with response.");
        }

        public static IEnumerable<IAssertion> GettingList()
        {
            // Request
            yield return new VerifyRequestMethod(HttpMethod.Get)
               .Expected("Convention: HTTP request method is GET");

            // Response
            yield return new VerifyResponseStatus(HttpStatusCode.OK)
               .Expected("Convention: Returned HTTP status code is 200 (OK)");
        }
    }
}