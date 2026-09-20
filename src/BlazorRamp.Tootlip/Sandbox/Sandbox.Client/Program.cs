using BlazorRamp.Core.Common.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazorRampCore();
await builder.Build().RunAsync();
