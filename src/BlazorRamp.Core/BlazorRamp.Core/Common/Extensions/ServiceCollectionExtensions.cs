using BlazorRamp.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.Core.Common.Extensions;

/// <summary>
/// Provides extension methods for registering BlazorRamp Core services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the core BlazorRamp services with the application's dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.
    /// </returns>
    public static IServiceCollection AddBlazorRampCore(this IServiceCollection services)
    {
        services.AddScoped<ILiveRegionService, LiveRegionService>();
        return services;
    }
}