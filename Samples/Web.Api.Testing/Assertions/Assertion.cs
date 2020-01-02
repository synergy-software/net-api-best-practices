using Synergy.Contracts;
using Synergy.Samples.Web.API.Tests.WAPIT.Features;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Assertions
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