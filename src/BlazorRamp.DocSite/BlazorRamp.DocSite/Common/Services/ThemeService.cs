using BlazorRamp.DocSite.Common.Constants;
using Microsoft.JSInterop;

namespace BlazorRamp.DocSite.Common.Services;

public class ThemeService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Doc_Theme_Module_File_Path).AsTask());


    public async Task<string> GetPropertyValue(string propertyName)
    {
        var module = await _moduleTask.Value;

        return await module.InvokeAsync<string>(GlobalValues.JS_Theme_Get_Comp_Style_Property_Func, propertyName);
    }
    public async Task RemovePropertyValue(string propertyName)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync(GlobalValues.JS_Theme_Remove_Style_Property_Func, propertyName);
    }
    public async Task SetPropertyValue(string propertyName, string value)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync(GlobalValues.JS_Theme_Set_Style_Property_Func, propertyName, value);
    }
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated) await (await _moduleTask.Value).DisposeAsync();
    }
}
