using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Web.Extensions;
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
                    .AddNewtonsoftJson(options =>
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