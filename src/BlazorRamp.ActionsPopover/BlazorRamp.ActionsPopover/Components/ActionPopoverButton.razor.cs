using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using BlazorRamp.ActionsPopover.Common.Utilities;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.ActionsPopover.Components;

partial class ActionPopoverButton<TData>
{
    [Parameter] public string     ButtonText { get; set; }
    [Parameter] public TData?     ItemData   { get; set; }


    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Invoked when the action button is clicked. Unlike <see cref="EventCallback"/>, this does
    /// <b>not</b> automatically trigger a re-render of the consuming component — call
    /// <c>StateHasChanged()</c> yourself if the action needs to update the UI.
    /// </summary>
    [Parameter] public Func<ButtonActionData<TData>, Task>? OnClick { get; set; }

    [CascadingParameter(Name = $"{GlobalValues.Actions_Popover_Panel_Cascading_ID_Name}")]
    private string? PopoverID { get; set; }



    private string? _svgIcon = null;
    protected override void OnParametersSet()
    {
        if (true == String.IsNullOrWhiteSpace(ButtonText)) throw new ArgumentNullException(nameof(ButtonText), GlobalValues.Actions_Popover_Button_Text_Exception_Message);
        _svgIcon = GeneralHelpers.CheckSetSvgVariable(SvgIcon, GlobalValues.Actions_Popover_Action_Icon_Svg_Css_Variable_Name);
        base.OnParametersSet();
    }


    private async Task RaiseOnClick(string buttonText, TData? itemData)
    {
        var actionData = CreateActionData(buttonText, itemData);
        if (OnClick is not null) await OnClick.Invoke(actionData);
    }

    private static ButtonActionData<TData> CreateActionData(string buttonText, TData? payload = default(TData))

        => new(buttonText, payload);

}
