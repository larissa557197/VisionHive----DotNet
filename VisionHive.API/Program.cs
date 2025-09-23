using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VisionHive.Application;
using VisionHive.Infrastructure;
using VisionHive.Infrastructure.Contexts;
namespace VisionHive.API;

public class Program
{
    public static void Main(string[] args)
    {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers + JSON options
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
            
            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "API para cadastro de Motos e areas",
                    Version = "v1",
                    Description = "API desenvolvida para a empresa Mottu - Projeto Vision Hive\n\n" +
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
            });
                
            // registra Infra (DbContext + repositórios ) e Application (use cases)
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
