using JetBrains.Annotations;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Features
{
    public static class FeatureExtensions
    {
        public static HttpOperation InStep([NotNull] this HttpOperation operation, [NotNull] Step step)
        {
            step.Attach(operation);
            return operation;
        }
    }
}