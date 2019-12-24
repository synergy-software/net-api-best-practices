using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sample.API;

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
                    });
    }
}
