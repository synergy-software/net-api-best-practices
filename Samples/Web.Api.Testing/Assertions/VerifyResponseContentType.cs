using System;
using Synergy.Contracts;

namespace Synergy.Web.Api.Testing.Assertions
{
    public class VerifyResponseContentType : Assertion
    {
        private readonly string _expectedContentType;

        public VerifyResponseContentType(string expectedContentType)
        {
            _expectedContentType = expectedContentType.NotNull(nameof(expectedContentType));
            ExpectedResult = $"Returned HTTP Content-Type is \"{_expectedContentType}\"";
        }

        public override void Assert(HttpOperation operation)
        {
            var actualContentType = operation.Response.Content.Headers.ContentType.MediaType;
            Fail.IfNotEqual(
                _expectedContentType,
                actualContentType,
                Violation.Of(
                    "Expected HTTP Content-Type is \"{0}\" but was \"{1}\" in response:{2}{2}{3}",
                    _expectedContentType,
                    actualContentType,
                    Environment.NewLine,
                    operation.Response.ToHttpLook()
                    )
                );
        }
    }
}