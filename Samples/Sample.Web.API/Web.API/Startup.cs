using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging(
                options =>
                {
                    // Customize the message template
                    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

                    // Emit debug-level events instead of the defaults
                    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                    // Attach additional properties to the request completion event
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                                                      {
                                                          diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                                                          diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                                                          diagnosticContext.Set("Environment", env.EnvironmentName);
                                                          diagnosticContext.Set("Application", env.ApplicationName);
                                                      };
                });

            app.UseSwagger()
               .UseVersionedSwaggerUI();

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