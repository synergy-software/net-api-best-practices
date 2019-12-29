namespace Synergy.Samples.Web.API.Tests.WAPIT.Assertions
{
    public interface IAssertion
    {
        void Assert(HttpOperation operation);
    }
}