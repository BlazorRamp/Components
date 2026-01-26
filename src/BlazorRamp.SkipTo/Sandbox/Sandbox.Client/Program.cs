using BlazorRamp.Core.Common.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Sandbox.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddBlazorRampCore();
            await builder.Build().RunAsync();
        }
    }
}
