using System.Net.Http;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Assertions
{
    public class VerifyRequestMethod : Assertion
    {
        private readonly HttpMethod _expectedMethod;

        public VerifyRequestMethod(HttpMethod expectedMethod)
        {
            _expectedMethod = expectedMethod;
            ExpectedResult = $"HTTP request method is {_expectedMethod}";
        }

        public override void Assert(HttpOperation operation)
        {
            var actualMethod = operation.Request.Method;
            Fail.IfNotEqual(_expectedMethod, actualMethod,
                            Violation.Of($"Expected HTTP method is {_expectedMethod} but was {actualMethod}")
                           );
        }
    }
}