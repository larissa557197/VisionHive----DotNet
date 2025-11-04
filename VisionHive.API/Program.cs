using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using VisionHive.Application;
using VisionHive.Infrastructure;
using VisionHive.API.Extensions;
using VisionHive.Application.Configs;



namespace VisionHive.API;
public class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);
        
       // carrega as configurações combinadas (appsettings + appsettings.Development)
       var settings = builder.Configuration.Get<Settings>();
       
        // Controllers + JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler =
                    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

        // Swagger (usa configuraçções do appsettings.json)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger(settings.Swagger);
        
        // versionamento da API
        builder.Services.AddVersioning();
        
        // Health Checks (Oracle + MongoDB + URLS)
        builder.Services.AddChecks(settings);
        
        // Dependency Injection das camadas
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddUseCases();
        
        // CORS (libera para todos os dominios
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

            });
        });
        
        // Autenticação (JWT) - usa a SecretKey do appsettingsDevelopment.json
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),

                };

            });

        
        builder.Services.AddAuthorization();


        var app = builder.Build();

        // Swagger (V1 + V2)
        app.UseSwagger();
        app.UseSwaggerUI(ui =>
        {
            ui.SwaggerEndpoint("/swagger/v1/swagger.json", "VisionHive API v1 (Oracle)");
            ui.SwaggerEndpoint("/swagger/v2/swagger.json", "VisionHive API v2 (MongoDB + JWT)");
            ui.RoutePrefix = "swagger";
            ui.DocumentTitle = "VisionHive API";

            // Desabilita validação externa
            ui.ConfigObject.AdditionalItems["validatorUrl"] = null;

            //  Injeta script para forçar ícones de bloqueio no Swagger UI
            ui.InjectJavascript("/swagger-lock-fix.js");
        });


        // middlewares principais
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();

        // controllers + healthcheck
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckExtensions.WriteResponse
        });

        app.Run();
    }
}