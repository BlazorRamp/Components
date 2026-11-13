using BlazorRamp.Notifications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.Notifications.Common.Extensions;

/// <summary>
/// Provides extension methods for registering the BlazorRamp Notification service.
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Registers the BlazorRamp Notification service with the application's dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.
    /// </returns>
    public static IServiceCollection AddBlazorRampNotifications(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService> ();

        return services;
    }
}
