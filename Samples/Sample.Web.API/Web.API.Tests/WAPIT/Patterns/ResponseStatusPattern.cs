using System.Net;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public class ResponseStatusPattern : IPattern
    {
        private readonly HttpStatusCode _expectedStatus;

        public ResponseStatusPattern(HttpStatusCode expectedStatus)
        {
            _expectedStatus = expectedStatus;
        }

        public void Equals(HttpOperation operation)
        {
            var actualStatus = operation.Response.StatusCode;
            Fail.IfNotEqual(_expectedStatus, actualStatus,
                            Violation.Of($"Expected HTTP status is {_expectedStatus} but was {actualStatus}")
                           );
        }
    }
}