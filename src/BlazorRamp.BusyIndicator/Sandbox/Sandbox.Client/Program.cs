using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Sandbox.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            await builder.Build().RunAsync();
        }
    }
}
