using EpsonProjectorComponent.Epson.Projector.Transport;
using EpsonProjectorComponent.Epson.Projector.Transport.Auth;
using EpsonProjectorComponent.Services;

namespace EpsonProjectorComponent.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEpsonProjectorRemote(this IServiceCollection services)
    {
        services.AddScoped<DigestAuthStrategy>();
        services.AddScoped<NoAuthStrategy>();
        services.AddScoped<SimulatedProjectorTransport>();
        services.AddScoped<HttpProjectorTransport>();
        services.AddScoped<ProjectorControllerService>();

        return services;
    }
}
