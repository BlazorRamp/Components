using BlazorRamp.Notifications.Common.Extensions;
using BlazorRamp.Core.Common.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorRampNotifications();
builder.Services.AddBlazorRampCore();
await builder.Build().RunAsync();
