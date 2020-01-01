using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Tests.WAPIT.Assertions;

namespace Synergy.Samples.Web.API.Tests.Infrastructure
{
    public class ApiConventionFor
    {
        /// <summary>
        /// Gets set of assertions that verify if created element is returned properly.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IAssertion> Create()
        {
            // Request
            yield return new VerifyRequestMethod(HttpMethod.Post);

            // Response
            yield return new VerifyResponseStatus(HttpStatusCode.Created);
            yield return new VerifyResponseHeader("Location", value => Fail.IfWhitespace(value, Violation.Of("There is no 'Location' header returned")))
               .Expected("Location header (pointing to newly created element) is returned with response.");
        }
    }
}