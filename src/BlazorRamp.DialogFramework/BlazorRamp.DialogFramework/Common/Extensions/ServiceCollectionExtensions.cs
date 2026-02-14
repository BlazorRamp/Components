using BlazorRamp.DialogFramework.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.DialogFramework.Common.Extensions;

/// <summary>
/// Extension methods for registering Blazor Ramp Dialog Framework services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="ModalDialogService"/> with the dependency injection container.
    /// Call this in your application's service configuration alongside adding the
    /// <c>&lt;ModalDialogContainer /&gt;</c> component to your router layout.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add services to.
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> for chaining.
    /// </returns>
    public static IServiceCollection AddBlazorRampDialogService(this IServiceCollection services)
    {
        services.AddScoped<ModalDialogService>();
        return services;
    }
}
