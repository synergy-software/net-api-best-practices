using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Synergy.Samples.Web.API.Extensions;

namespace Sample.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration(
                    (hostingContext, config) =>
                    {
                        var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                        config.AddJsonFile($"appsettings.{environmentName}.json", true);
                        config.AddEnvironmentVariables();
                    })
                .UseServiceProviderFactory(new WindsorServiceProviderFactory())
                .ConfigureContainer<WindsorContainer>(
                    (hostBuilderContext, container) =>
                    {
                        container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
                        container.AddFacility<TypedFactoryFacility>();

                        var rootAssembly = Application.GetRootAssembly();
                        container.Register(
                            Classes
                                .FromAssemblyInThisApplication(rootAssembly)
                                .Pick()
                                .Unless(x => x.GetInterfaces().IsEmpty() || x.IsConstructable() == false)
                                .WithServiceAllInterfaces()
                                .LifestyleSingleton()
                        );

                        // Execute all installers in every library in the application
                        container.Install(FromAssembly.InThisApplication(rootAssembly));
                    })
        ;
    }
}
