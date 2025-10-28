using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VisionHive.Infrastructure.Contexts;
using VisionHive.Infrastructure.Repositories;
using VisionHive.Infrastructure.Repositories.Mongo;

namespace VisionHive.Infrastructure;

public static class DependencyInjection
{
    // 🔹 DbContext (Oracle)
    private static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<VisionHiveContext>(options =>
        {
            var cs = configuration.GetConnectionString("DefaultConnection") 
                     ?? configuration.GetConnectionString("Oracle");
            options.UseOracle(cs);
        });
    }

    // 🔹 Contexto e dependências do MongoDB
    private static IServiceCollection AddMongoContext(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetSection("MongoDb:ConnectionString").Value;
        var mongoDatabase = configuration.GetSection("MongoDb:DatabaseName").Value;

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnection));
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDatabase);
        });

        return services;
    }

    // 🔹 Repositórios relacionais
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMotoRepository, MotoRepository>();
        services.AddScoped<IPatioRepository, PatioRepository>();
        services.AddScoped<IFilialRepository, FilialRepository>();

        // 🔹 Repositórios Mongo (para v2)
        services.AddScoped<MotoMongoRepository>();
        services.AddScoped<PatioMongoRepository>();
        services.AddScoped<FilialMongoRepository>();

        return services;
    }

    /// <summary>
    /// Método único para registrar DbContext + Mongo + Repositórios
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDBContext(configuration);
        services.AddMongoContext(configuration);
        services.AddRepositories();

        return services;
    }
}
