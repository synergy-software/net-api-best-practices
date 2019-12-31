using Synergy.Samples.Web.API.Tests.WAPIT.Features;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Assertions
{
    public interface IAssertion : IExpectation
    {
        void Assert(HttpOperation operation);
    }
}