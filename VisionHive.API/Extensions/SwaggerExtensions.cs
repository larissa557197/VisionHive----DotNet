using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VisionHive.Application.Configs;

namespace VisionHive.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerSettings settings)
        {
            return services.AddSwaggerGen(swagger =>
            {
                // Documentação da v1 (Oracle)
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = settings.Title,
                    Version = "v1",
                    Description = settings.Description,
                    Contact = settings.Contact
                });

                // Documentação da v2 (MongoDB + JWT)
                swagger.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = settings.Title + " v2",
                    Version = "v2",
                    Description = settings.Description + " (MongoDB + JWT)",
                    Contact = settings.Contact
                });

                // Servidores definidos no appsettings
                if (settings.Servers is not null)
                {
                    foreach (var server in settings.Servers)
                    {
                        swagger.AddServer(new OpenApiServer
                        {
                            Url = server.Url,
                            Description = server.Description
                        });
                    }
                }

                // Definição do esquema Bearer (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = " **Autenticação e autorização via JWT**\n\n" +
                                  " 1. Faça login usando o endpoint `/api/v2/auth/login`\n" +
                                  " 2. Copie o token retornado no campo `accessToken`\n" +
                                  " 3. Clique em *Authorize* e insira:\n\n" +
                                  "`Bearer {seu_token_aqui}`\n\n" +
                                  "O token expira em 1 hora.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });


                // Requisito de segurança (global)
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

                //  Força as rotas com [Authorize] a exibirem cadeado fechado
                swagger.OperationFilter<AuthorizeCheckOperationFilter>();

                //  Corrige bug do Swagger com versionamento múltiplo
                swagger.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                        return false;

                    var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(true)
                        .OfType<Asp.Versioning.ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    if (versions is null)
                        return docName == "v1"; // fallback

                    return versions.Any(v => $"v{v.MajorVersion}" == docName);
                });
            });
        }
    }

}


