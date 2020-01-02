using Synergy.Contracts;
using Synergy.Web.Api.Testing.Features;

namespace Synergy.Web.Api.Testing.Assertions
{
    public abstract class Assertion : IAssertion
    {
        public string? ExpectedResult { get; protected set; }

        public abstract void Assert(HttpOperation operation);

        public IAssertion Expected(string expected)
        {
            this.ExpectedResult = expected.OrFailIfWhiteSpace(nameof(expected));
            return this;
        }
    }

    public interface IAssertion : IExpectation
    {
        void Assert(HttpOperation operation);
    }
}