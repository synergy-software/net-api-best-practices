using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Web.Extensions;
using Serilog;
using Serilog.Events;
using Synergy.Samples.Web.API.Extensions;

namespace Sample.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
                    .AddNewtonsoftJson(
                         options =>
                         {
                             options.SerializerSettings.Converters.Add(new StringEnumConverter());
                             options.SerializerSettings.Formatting = Formatting.Indented;
                             options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                         });

            services.AddVersionedApi();
            services.AddVersionedSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime, ILogger<Startup> logger)
        {
            logger.LogDebug("Configuring request logging engine");

            // TODO: Additionaly log (trace/debug level) full request and response - with dedicated middleware/filter
            app.UseSerilogRequestLogging(
                options =>
                {
                    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                                                      {
                                                          diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                                                          diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                                                          diagnosticContext.Set("Environment", env.EnvironmentName);
                                                          diagnosticContext.Set("Application", env.ApplicationName);
                                                      };
                });

            logger.LogDebug("Configuring Swagger UI (Open API) engine");
            app.UseSwagger()
               .UseVersionedSwaggerUI();

            logger.LogDebug("Configuring Exception handling");
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //if (env.IsDevelopment())
            //    app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}