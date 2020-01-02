using System;
using Newtonsoft.Json.Linq;

namespace Synergy.Web.Api.Testing.Assertions {
    public class VerifyResponseBody : Assertion
    {
        private readonly string _jsonToken;
        private readonly Action<JToken?> _validate;

        public VerifyResponseBody(string jsonToken, Action<JToken?> validate)
        {
            _jsonToken = jsonToken;
            _validate = validate;
        }

        public override void Assert(HttpOperation operation)
        {
            var token = operation.Response.Content.ReadJson()?.SelectToken(_jsonToken);
            _validate(token);
        }
    }
}