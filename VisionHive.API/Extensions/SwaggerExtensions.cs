using Microsoft.OpenApi.Models;
using VisionHive.Application.Configs;

namespace VisionHive.API.Extensions;

public static class SwaggerExtensions
{
     public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerSettings settings)
    {
        return services.AddSwaggerGen(swagger =>
        {
            // Documentação v1 - Oracle
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = settings.Title,
                Version = "v1",
                Description = settings.Description,
                Contact = settings.Contact
            });

            // Documentação v2 - MongoDB
            swagger.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = settings.Title + " v2",
                Version = "v2",
                Description = settings.Description,
                Contact = settings.Contact
            });

            // Servidores (URLs) — adiciona dinamicamente os servers do appsettings.Development.json
            swagger.AddServer(new OpenApiServer());
            foreach (var server in settings.Servers)
            {
                swagger.AddServer(new OpenApiServer
                {
                    Url = server.Url,
                    Description = server.Description
                });
            }

            // Segurança (JWT)
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Autenticação e autorização via JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}

