using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Castle.Core.Internal;
using Microsoft.Extensions.Hosting;
using Synergy.Contracts;

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

        public static Info GetApplicationInfo()
        {
            var assembly = GetRootAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var createdOn = File.GetLastWriteTime(assembly.Location);

            return new Info(fileVersionInfo, createdOn);
        }

        /// <summary>
        /// Checks if the current host environment name is <see cref="Application.Environment.Tests"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="Application.Environment.Tests"/>, otherwise false.</returns>
        public static bool IsTests(this IHostEnvironment hostEnvironment)
        {
            Fail.IfNull(hostEnvironment, nameof(hostEnvironment));

            return hostEnvironment.IsEnvironment(Environment.Tests);
        }

        public static class Environment
        {
            public const string Tests = "Tests";
        }

        public struct Info
        {
            public string ProductName { get; }
            public string FileVersion { get; }
            public DateTime CreatedOn { get; }

            public Info(FileVersionInfo fileVersionInfo, DateTime createdOn)
            {
                ProductName = fileVersionInfo.ProductName;
                FileVersion = fileVersionInfo.FileVersion;
                CreatedOn = createdOn;
            }
        }
    }
}