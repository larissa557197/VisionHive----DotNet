using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using VisionHive.Application.Configs;

namespace VisionHive.API.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddChecks(this IServiceCollection services, Settings settings)
    {
        // Mongo Client
        services.AddSingleton(sp =>
            new MongoClient(settings.MongoDb.ConnectionString));

        services.AddHealthChecks()
            .AddOracle(
                connectionString: settings.ConnectionStrings.DefaultConnection,
                name: "Oracle",
                tags: new[] { "db", "oracle" }
            )
            .AddMongoDb(
                sp => new MongoClient(settings.MongoDb.ConnectionString),
                name: "MongoDB",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "mongo" }
            )


            .AddUrlGroup(new Uri("https://fiap.com.br"), "FIAP")
            .AddUrlGroup(new Uri("https://google.com.br"), "Google");


        return services;
    }
    
    // método que formata JSON da resposta /health
    public static Task WriteResponse(HttpContext context, HealthReport report)
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        string json = JsonSerializer.Serialize(
            new
            {
                Status = report.Status.ToString(),
                Duration = report.TotalDuration,
                Info = report.Entries.Select(entry => new
                {
                    entry.Key,
                    entry.Value.Description,
                    entry.Value.Duration,
                    Status = Enum.GetName(typeof(HealthStatus), entry.Value.Status),
                    Error = entry.Value.Exception?.Message,
                    entry.Value.Data
                }).ToList()
            },
            jsonSerializerOptions
        );

        context.Response.ContentType = MediaTypeNames.Application.Json;

        return context.Response.WriteAsync(json);
    }
}