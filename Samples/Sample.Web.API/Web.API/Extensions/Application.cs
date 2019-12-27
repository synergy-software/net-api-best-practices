﻿using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Castle.Core.Internal;
using Microsoft.Extensions.Hosting;

namespace Synergy.Samples.Web.API.Extensions
{
    public static class Application
    {
        public static Assembly GetRootAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        public static ReadOnlyCollection<Assembly> GetApplicationAssemblies()
        {
            return ReflectionUtil.GetApplicationAssemblies(GetRootAssembly()).AsReadOnly();
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="Application.Environment.Tests"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="Application.Environment.Tests"/>, otherwise false.</returns>
        public static bool IsTests(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null) throw new ArgumentNullException(nameof(hostEnvironment));

            return hostEnvironment.IsEnvironment(Environment.Tests);
        }

        public static class Environment
        {
            public const string Tests = "Tests";
        }
    }
}
