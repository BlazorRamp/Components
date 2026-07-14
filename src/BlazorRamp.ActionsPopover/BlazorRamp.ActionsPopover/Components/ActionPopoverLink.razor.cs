using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using BlazorRamp.ActionsPopover.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorRamp.ActionsPopover.Components;

/// <summary>
/// Represents a single link action item rendered within an <see cref="ActionsPopover"/> panel.
/// Renders as an anchor element so standard link behaviours (open in new tab, copy link,
/// browser context menu) keep working, while still raising an <see cref="OnClick"/> callback.
/// </summary>
public partial class ActionPopoverLink<TData> : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the text displayed for this action link.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the value is <see langword="null"/>, empty, or consists only of whitespace.
    /// </exception>
    [Parameter] public string     LinkText       { get; set; } = default!;

    /// <summary>
    /// Gets or sets an optional payload associated with this action, made available via
    /// <see cref="LinkActionData{TData}.GetValueOr(TData)"/> in the <see cref="OnClick"/> callback.
    /// </summary>
    [Parameter] public TData?     ItemData       { get; set; }

    /// <summary>
    /// Gets or sets the browsing context the link opens in. Defaults to <see cref="TargetType.Self"/>.
    /// </summary>
    [Parameter] public TargetType TargetType     { get; set; } = TargetType.Self;

    /// <summary>
    /// Gets or sets the <c>href</c> for this link. Defaults to <c>"/"</c>.
    /// </summary>
    [Parameter] public string     Path           { get; set; } = "/";

    /// <summary>
    /// Gets or sets whether the default navigation behaviour of the link is suppressed,
    /// allowing the consumer to handle navigation manually via <see cref="OnClick"/>.
    /// Evaluated once on initialization; changing this after the first render has no effect.
    /// </summary>
    [Parameter] public bool       PreventDefault { get; set; } = false;

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Gets or sets a CSS colour value (hex, named colour, or <c>var(--token)</c> reference) used to
    /// tint this action item's icon. Has no effect when forced colours mode (e.g. Windows High Contrast)
    /// is active, since accessibility requires the system-defined colour to take precedence.
    /// </summary>
    [Parameter] public string? IconColour { get; set; }


    /// <summary>
    /// Invoked when the action button is clicked. Unlike <see cref="EventCallback"/>, this does
    /// <b>not</b> automatically trigger a re-render of the consuming component — call
    /// <c>StateHasChanged()</c> yourself if the action needs to update the UI.
    /// </summary>
    [Parameter] public Func<LinkActionData<TData>, Task>? OnClick { get; set; }

    [CascadingParameter(Name = $"{GlobalValues.Actions_Popover_Panel_Cascading_ID_Name}")]
    private string? PopoverID { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    private IJSObjectReference? _jSModule = null;

    private ElementReference AnchorElementRef { get; set; }

    private string? _svgIcon        = null;
    private bool    _preventDefault = false;
    private string   _path          = "/";
    private string?  _iconColour    = null;
    private string   _targetType    = "_self";
    /// <summary>
    /// Validates <see cref="LinkText"/> and resolves <see cref="SvgIcon"/> into a CSS custom property style.
    /// </summary>
    protected override void OnParametersSet()
    {
        if (true == String.IsNullOrWhiteSpace(LinkText)) throw new ArgumentNullException(nameof(LinkText), GlobalValues.Actions_Popover_Link_Text_Exception_Message);
       
        _path       = String.IsNullOrWhiteSpace(Path) ? "/" : Path.Trim();
        _svgIcon    = GeneralHelpers.CheckSetSvgVariable(SvgIcon, GlobalValues.Actions_Popover_Action_Icon_Svg_Css_Variable_Name);
        _iconColour = GeneralHelpers.CheckSetColourVariable(IconColour, GlobalValues.Actions_Popover_Action_Icon_Colour_Variable_Name);
        _targetType = GeneralHelpers.GetTargetType(TargetType);

        base.OnParametersSet();
    }

    /// <summary>
    /// Captures the initial value of <see cref="PreventDefault"/> for use during
    /// <see cref="OnAfterRenderAsync"/>, since JS registration only happens once on first render.
    /// </summary>
    protected override void OnInitialized()
    {
        _preventDefault = PreventDefault;
    }

    private async Task RaiseOnClick(string linkText, TData? itemData, TargetType targetType, string path)
    {
        var actionData = CreateActionData(linkText, itemData, targetType, path);

        if (_jSModule != null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Hide_Popover_Func, PopoverID);

        if (OnClick is not null) await OnClick.Invoke(actionData);
    }

    private static LinkActionData<TData> CreateActionData(string linkText, TData? payload, TargetType targetType, string path)

        => new(linkText, payload, targetType, path);

    /// <summary>
    /// Imports the JavaScript module and registers the prevent click action handler 
    /// to stop the default href action as we are using async not sync
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        if (true == firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_File_Path);

            
            if (_jSModule is not null && true == _preventDefault ) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Prevent_Click_Action_Handler, AnchorElementRef);

        }
    }

    /// <summary>
    /// Releases the JavaScript module reference and unregisters the prevent click action handler registered during
    /// <c>OnAfterRenderAsync</c>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {

        if (_jSModule is not null)
        {
            try
            {
                if(true == _preventDefault )await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Prevent_Click_Action_Handler, AnchorElementRef);
                await _jSModule.DisposeAsync();

            }
            catch { }
        }

    }
}
