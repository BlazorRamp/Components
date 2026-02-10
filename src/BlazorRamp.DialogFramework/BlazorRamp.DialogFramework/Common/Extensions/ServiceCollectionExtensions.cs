using BlazorRamp.DialogFramework.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.DialogFramework.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorRampDialogFramework(this IServiceCollection services)
    {
        services.AddScoped<ModalDialogService>();
        return services;
    }
}
