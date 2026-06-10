using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorRamp.Core.Common.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorRampCore();

await builder.Build().RunAsync();
