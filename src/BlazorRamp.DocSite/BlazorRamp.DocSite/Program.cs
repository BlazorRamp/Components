using BlazorRamp.Core.Common.Extensions;
using BlazorRamp.DialogFramework.Common.Extensions;
using BlazorRamp.DocSite.Common.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorRamp.DocSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddBlazorRampCore();
            builder.Services.AddBlazorRampDialogService();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<ThemeService>();
            await builder.Build().RunAsync();
        }
    }
}
