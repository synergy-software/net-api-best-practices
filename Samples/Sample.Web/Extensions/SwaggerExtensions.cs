using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Sample.Web.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddVersionedSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

                // Add a swagger document for each discovered API version  
                foreach (var apiVersion in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc("v1", GenerateSwaggerVersionInfo(apiVersion, environment));
                }

                // TODO: Dodaj filtry

                // TODO: Dodaj chodzenie po wszystkich bibliotekach w projekcie w poszukiwaniu plików xml - Librarian
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.CustomSchemaIds(GetSchemaId);
            });
        }

        private static OpenApiInfo GenerateSwaggerVersionInfo(ApiVersionDescription api, IWebHostEnvironment environment)
        {
            return new OpenApiInfo
            {
                Version = api.ApiVersion.ToString(),
                Title = "Synergy sample API",
                Description = GetApiVersionDescription(api, environment),
                //TermsOfService = new Uri("https://github.com/synergy-software/net-api-best-practices/blob/master/LICENSE"),
                Contact = new OpenApiContact
                {
                    Name = "Synergy software",
                    Email = "synergy@todo.com",
                    Url = new Uri("https://github.com/synergy-software")
                },
                License = new OpenApiLicense
                {
                    Name = "Use under MIT License",
                    Url = new Uri("https://github.com/synergy-software/net-api-best-practices/blob/master/LICENSE")
                }
            };
        }

        private static string GetApiVersionDescription(ApiVersionDescription description, IWebHostEnvironment environment)
        {
            // TODO: wydziel klasę / komponent odpowiedzialną za pobranie informacji o aplikacji
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var createdOn = File.GetLastWriteTime(assembly.Location).ToString();
            if (environment.IsDevelopment())
                createdOn = "DEVELOPERS MACHINE";

            return $"<label>API Version</label>: <strong>{description.ApiVersion} {(description.IsDeprecated ? "(DEPRECATED)" : "")}</strong><br/> " +
                   $"<label>Application Name</label>: <strong>{fileVersionInfo.ProductName}</strong><br/> " +
                   $"<label>Application Version</label>: <strong>{fileVersionInfo.FileVersion}</strong><br/> " +
                   $"<label>Application Created on</label>: <strong>{createdOn}</strong>";
        }

        private static string GetSchemaId(Type type)
        {
            if (type.DeclaringType == null)
            {
                return type.Name;
            }

            return GetSchemaId(type.DeclaringType) + "." + type.Name;
        }

        public static void UseVersionedSwaggerUI(this IApplicationBuilder app)
        {
            var apiVersionProvider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(
                c =>
                {
                    // Build a swagger endpoint for each discovered API version
                    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                }
            );
        }
    }
}