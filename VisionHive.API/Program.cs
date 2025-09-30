using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VisionHive.Application;
using VisionHive.Infrastructure;
using VisionHive.Infrastructure.Contexts;
using Swashbuckle.AspNetCore.Filters;
namespace VisionHive.API;

public class Program
{
    public static void Main(string[] args)
    {
       var builder = WebApplication.CreateBuilder(args);

        // Controllers + JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler =
                    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

        // Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "API para cadastro de Motos e áreas",
                Version = "v1",
                Description =
                    "API desenvolvida para a empresa Mottu - Projeto Vision Hive\n\n" +
                    "Integrantes:\n" +
                    " Larissa Muniz (RM557197) \n" +
                    " Joao Victor Michaeli (RM555678) \n" +
                    " Henrique Garcia (RM558062) ",
            });

            // comentários XML para gerar documentação dos métodos/classes
            var xmls = new[]
            {
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
                "VisionHive.Application.xml",
                "VisionHive.Domain.xml",
                "VisionHive.Infrastructure.xml"
            };

            foreach (var file in xmls)
            {
                var path = Path.Combine(AppContext.BaseDirectory, file);
                if (File.Exists(path))
                    swagger.IncludeXmlComments(path);
            }

            // habilita exemplos (request/response) no Swagger
            swagger.ExampleFilters();
        });

        // registra os providers de exemplos a partir deste assembly
        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

        // registra Infra (DbContext + repositórios) e Application (use cases)
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();

        var app = builder.Build();

        // HTTP pipeline - Swagger sempre ativo enquanto testa
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // usa caminho ABSOLUTO correto
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "VisionHive API v1");
            // mude o prefixo para forçar uma nova URL (evita cache do index.html)
            c.RoutePrefix = "docs";
            c.DocumentTitle = "VisionHive API";
            // desliga o validador externo do swagger.io
            c.ConfigObject.AdditionalItems["validatorUrl"] = null;
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}