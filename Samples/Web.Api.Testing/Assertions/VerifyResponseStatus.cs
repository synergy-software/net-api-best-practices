using System.Net;
using Synergy.Contracts;

namespace Synergy.Web.Api.Testing.Assertions
{
    public class VerifyResponseStatus : Assertion
    {
        private readonly HttpStatusCode _expectedStatus;

        public VerifyResponseStatus(HttpStatusCode expectedStatus)
        {
            _expectedStatus = expectedStatus;
            ExpectedResult = $"Returned HTTP status code is {(int)_expectedStatus} ({_expectedStatus})";
        }

        public override void Assert(HttpOperation operation)
        {
            var actualStatus = operation.Response.StatusCode;
            Fail.IfNotEqual(_expectedStatus, actualStatus,
                            Violation.Of($"Expected HTTP status is {_expectedStatus} but was {actualStatus}")
                           );
        }
    }
}