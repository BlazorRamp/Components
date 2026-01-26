using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.SkipTo.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorRamp.SkipTo.Components.SkipTo;

public partial class SkipTo : IAsyncDisposable
{
    [Parameter] public string SkipToText     { get; set; } = GlobalValues.SkipTo_Text;
    [Parameter] public string TargetID       { get; set; }  = GlobalValues.SkipTo_Target_ID;
    [Parameter] public SkipToType SkipToType { get; set; } = SkipToType.Page;
    [Parameter] public bool IconVisible      { get; set; } = true;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime       JSRuntime          { get; set; } = default!;

    private IJSObjectReference? _skipToModule;

    private string _skipToText    = GlobalValues.SkipTo_Text;
    private string _targetID      = GlobalValues.SkipTo_Target_ID;
    private bool   _iconVisible   = true;
    private bool   _isInteractive = false;
    private bool    _disposed     = false;
    protected override void OnParametersSet()
    {
        _skipToText  = String.IsNullOrWhiteSpace(SkipToText) ? GlobalValues.SkipTo_Text : SkipToText.Trim();
        _targetID    = String.IsNullOrWhiteSpace(TargetID) ? GlobalValues.SkipTo_Target_ID : TargetID.Trim();
        _targetID    = TargetID.StartsWith("#") ? _targetID : $"#{_targetID}";

        _iconVisible = IconVisible;
    }

    private string? BuildClassList(SkipToType skipToType)
    
        => skipToType == SkipToType.Section ? CoreUtilities.CreateClassList(GlobalValues.SkipTo_Class, GlobalValues.SkipTo_Container_Modifier) : GlobalValues.SkipTo_Class;
    

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        _isInteractive = true;

        if (true == firstRender)
        {
            _skipToModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_SkipTo_File_Path);
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task HandleNavigation(string navigateTo)
    {
        if (String.IsNullOrWhiteSpace(navigateTo)) return;
     

        var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri) + navigateTo;

        NavigationManager.NavigateTo(url, false, false);

        if (_skipToModule is not null) await _skipToModule.InvokeVoidAsync(GlobalValues.JS_SkipTo_Scroll_To_View_Func, navigateTo.TrimStart('#'));
    }


    public async ValueTask DisposeAsync()
    {
       if (_disposed == false && _skipToModule is not null) await _skipToModule.DisposeAsync();
       _disposed = true;
    }
}
