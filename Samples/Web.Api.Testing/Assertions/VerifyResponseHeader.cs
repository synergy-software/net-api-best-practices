using System;

namespace Synergy.Web.Api.Testing.Assertions
{
    public class VerifyResponseHeader : Assertion
    {
        private readonly string _headerName;
        private readonly Action<string> _validate;

        public VerifyResponseHeader(string headerName, Action<string> validate)
        {
            _headerName = headerName;
            _validate = validate;
        }

        public override void Assert(HttpOperation operation)
        {
            operation.Response.Headers.TryGetValues(_headerName, out var values);
            values ??= new string[1];
            foreach (var value in values) _validate(value);
        }
    }
}