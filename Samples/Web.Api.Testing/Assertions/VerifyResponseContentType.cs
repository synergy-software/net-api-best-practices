using Synergy.Contracts;

namespace Synergy.Web.Api.Testing.Assertions
{
    public class VerifyResponseContentType : Assertion
    {
        private readonly string _expectedContentType;

        public VerifyResponseContentType(string expectedContentType)
        {
            _expectedContentType = expectedContentType;
            ExpectedResult = $"Returned HTTP Content-Type is \"{_expectedContentType}\"";
        }

        public override void Assert(HttpOperation operation)
        {
            var actualContentType = operation.Response.Content.Headers.ContentType.MediaType;
            Fail.IfNotEqual(_expectedContentType, actualContentType,
                            Violation.Of($"Expected HTTP Content-Type is \"{_expectedContentType}\" but was {actualContentType}")
                           );
        }
    }
}