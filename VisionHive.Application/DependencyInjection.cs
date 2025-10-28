using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using FluentValidation;
using VisionHive.Application.Configs;
using VisionHive.Application.UseCases;
using VisionHive.Infrastructure.Repositories;
using VisionHive.Infrastructure.Contexts;
using VisionHive.Infrastructure.Repositories.Mongo;

namespace VisionHive.Application;

public static class DependencyInjection
{
    // Contexto Relacional (Oracle)
    private static IServiceCollection AddDBContext(this IServiceCollection services, Settings settings)
    {
        return services.AddDbContext<VisionHiveContext>(options =>
        {
            options.UseOracle(settings.ConnectionStrings.DefaultConnection);
        });
    }

    // Contexto NoSQL (MongoDB)
    private static IServiceCollection AddMongoContext(this IServiceCollection services, Settings settings)
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.MongoDb.ConnectionString));
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.MongoDb.DatabaseName);
        });

        return services;
    }

    // Repositórios
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Repositórios relacionais
        services.AddScoped<IMotoRepository, MotoRepository>();
        services.AddScoped<IFilialRepository, FilialRepository>();
        services.AddScoped<IPatioRepository, PatioRepository>();

        // Repositórios MongoDB
        services.AddScoped<FilialMongoRepository>();
        services.AddScoped<PatioMongoRepository>();
        services.AddScoped<MotoMongoRepository>();

        return services;
    }

    // Validadores (FluentValidation)
    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }

    // Application Layer (UseCases e Validadores)
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // UseCases
        services.AddScoped<IMotoUseCase, MotoUseCase>();
        services.AddScoped<IPatioUseCase, PatioUseCase>();
        services.AddScoped<IFilialUseCase, FilialUseCase>();

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }

    // 🔹 6. Infrastructure Layer (Contextos e Repositórios)
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Settings settings)
    {
        AddDBContext(services, settings);
        AddMongoContext(services, settings);
        AddRepositories(services);
        AddValidators(services);

        return services;
    }
}
