using System.Net;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public class VerifyResponseStatus : IAssertion
    {
        private readonly HttpStatusCode _expectedStatus;

        public VerifyResponseStatus(HttpStatusCode expectedStatus)
        {
            _expectedStatus = expectedStatus;
        }

        public void Assert(HttpOperation operation)
        {
            var actualStatus = operation.Response.StatusCode;
            Fail.IfNotEqual(_expectedStatus, actualStatus,
                            Violation.Of($"Expected HTTP status is {_expectedStatus} but was {actualStatus}")
                           );
        }
    }
}