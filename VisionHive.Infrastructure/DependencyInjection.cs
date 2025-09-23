using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisionHive.Infrastructure.Contexts;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.Infrastructure;

public static class DependencyInjection
{
    //registra o DbContext (Oracle)
    private static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<VisionHiveContext>(options =>
        {
            var cs = configuration.GetConnectionString("Oracle");
            options.UseOracle(cs);
        });
    }
    
    // Registra os repositórios da camada Infrastructure
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMotoRepository, MotoRepository>();
        services.AddScoped<IPatioRepository, PatioRepository>();
        services.AddScoped<IFilialRepository, FilialRepository>();
        return services;
    }
    
    /// <summary>
    /// Método único para registrar DbContext + Repositórios, no padrão do professor.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDBContext(configuration);
        services.AddRepositories();
        return services;
    }
}