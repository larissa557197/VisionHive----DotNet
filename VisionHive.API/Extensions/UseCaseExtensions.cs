using VisionHive.Application.UseCases;

namespace VisionHive.API.Extensions;

public static class UseCaseExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        // registro dos UseCases principais
        services.AddScoped<IMotoUseCase, MotoUseCase>();
        services.AddScoped<IPatioUseCase, PatioUseCase>();
        services.AddScoped<IFilialUseCase, FilialUseCase>();
        
        return services;
    }
}
