using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VisionHive.Infrastructure.Contexts;
namespace VisionHive.API;

public class Program
{
    public static void Main(string[] args)
    {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Incluir os coment�rios no Swagger
                swagger.IncludeXmlComments(xmlPath);
            });

            // Configura��o do banco de dados
            builder.Services.AddDbContext<VisionHiveContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
            });

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
