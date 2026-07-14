using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using BlazorRamp.ActionsPopover.Common.Utilities;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.ActionsPopover.Components;
/// <summary>
/// Represents a single button action item rendered within an <see cref="ActionsPopover"/> panel.
/// Use this for actions handled entirely in code, such as Edit or Delete, as opposed to
/// <see cref="ActionPopoverLink{TData}"/> which renders an anchor for navigation.
/// </summary>
public partial class ActionPopoverButton<TData> : ComponentBase
{
    /// <summary>
    /// Gets or sets the text displayed for this action button.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the value is <see langword="null"/>, empty, or consists only of whitespace.
    /// </exception>
    [Parameter] public string ButtonText { get; set; } = default!;

    /// <summary>
    /// Gets or sets an optional payload associated with this action, made available via
    /// <see cref="ButtonActionData{TData}.GetValueOr(TData)"/> in the <see cref="OnClick"/> callback.
    /// </summary>
    [Parameter] public TData? ItemData   { get; set; }


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
    [Parameter] public Func<ButtonActionData<TData>, Task>? OnClick { get; set; }

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the components button element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter(Name = $"{GlobalValues.Actions_Popover_Panel_Cascading_ID_Name}")]
    private string? PopoverID { get; set; }

    private string? _svgIcon    = null;
    private string? _iconColour = null;
    /// <summary>
    /// Validates <see cref="ButtonText"/> and resolves <see cref="SvgIcon"/> into a CSS custom property style.
    /// </summary>
    protected override void OnParametersSet()
    {
        if (true == String.IsNullOrWhiteSpace(ButtonText)) throw new ArgumentNullException(nameof(ButtonText), GlobalValues.Actions_Popover_Button_Text_Exception_Message);
        
        _svgIcon    = GeneralHelpers.CheckSetSvgVariable(SvgIcon, GlobalValues.Actions_Popover_Action_Icon_Svg_Css_Variable_Name);
        _iconColour = GeneralHelpers.CheckSetColourVariable(IconColour, GlobalValues.Actions_Popover_Action_Icon_Colour_Variable_Name);

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
