using BlazorRamp.Core.Common.Extensions;
using BlazorRamp.DialogFramework.Common.Extensions;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorRampCore();
builder.Services.AddBlazorRampDialogService();
await builder.Build().RunAsync();
