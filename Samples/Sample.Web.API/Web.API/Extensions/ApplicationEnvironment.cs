using System;
using Microsoft.Extensions.Hosting;

namespace Synergy.Samples.Web.API.Extensions
{
    public static class ApplicationEnvironment
    {
        public const string Tests = "Tests";

        /// <summary>
        /// Checks if the current host environment name is <see cref="ApplicationEnvironment.Tests"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="ApplicationEnvironment.Tests"/>, otherwise false.</returns>
        public static bool IsTests(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null) throw new ArgumentNullException(nameof(hostEnvironment));

            return hostEnvironment.IsEnvironment(Tests);
        }
    }
}