using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorRamp.Core.Common.Extensions;

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
