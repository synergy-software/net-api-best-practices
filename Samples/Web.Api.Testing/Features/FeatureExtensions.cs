using JetBrains.Annotations;

namespace Synergy.Web.Api.Testing.Features
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