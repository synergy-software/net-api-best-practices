using System;
using System.Net.Http;
using Synergy.Contracts;

namespace Synergy.Web.Api.Testing.Assertions
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
            Fail.IfNotEqual(
                _expectedMethod,
                actualMethod,
                Violation.Of(
                    "Expected HTTP method is {0} but was {1} in request:" +
                    "{2}{2}{3}",_expectedMethod, actualMethod, Environment.NewLine, operation.Request.ToHttpLook())
                );
        }
    }
}