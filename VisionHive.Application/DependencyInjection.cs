using Microsoft.Extensions.DependencyInjection;
using VisionHive.Application.UseCases;

namespace VisionHive.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registra os casos de uso da aplicação (camada Application).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMotoUseCase, MotoUseCase>();
        services.AddScoped<IPatioUseCase, PatioUseCase>();
        services.AddScoped<IFilialUseCase, FilialUseCase>();
        return services;
    }
}