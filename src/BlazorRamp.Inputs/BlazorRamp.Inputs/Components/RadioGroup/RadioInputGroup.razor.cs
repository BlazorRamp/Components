using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components.RadioGroup;

/// <summary>
/// Renders an accessible group of radio buttons. Inherits validation state management,
/// and hint text from <see cref="InputTypeBase{TValue}"/>.
/// This component acts as a coordinator for its child <see cref="RadioInput{TValue}"/> 
/// components, ensuring they share the same name and value context.
/// </summary>
/// <typeparam name="TValue">The type of the value bound to the radio group.</typeparam>
[CascadingTypeParameter(nameof(TValue))]
public class RadioTypeInputGroup<TValue> : InputTypeBase<TValue>
{

    /// <summary>
    /// Gets or sets the child content containing the <see cref="RadioInput{TValue}"/> options.
    /// </summary>
    [Parameter] public RenderFragment? OptionValues { get; set; } = default;
    /// <summary>
    /// Gets or sets the layout orientation of the radio buttons within the group. 
    /// Defaults to <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter] public Orientation Orientation { get; set; } = Orientation.Horizontal;


#pragma warning disable BL0007

    /// <summary>
    /// Not supported on <see cref="RadioInputGroup{TValue}"/>. This parameter has no effect if set.
    /// </summary>
    [Parameter]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool AriaDisabled { get => false; set { } }

    /// <summary>
    /// Not supported on <see cref="RadioInputGroup{TValue}"/>. This parameter has no effect if set.
    /// </summary>
    [Parameter]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool ReadOnly { get => false; set { } }

    ///// <summary>
    ///// Not supported on <see cref="RadioInputGroup{TValue}"/>. This parameter has no effect if set.
    ///// </summary>
    //[Parameter]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override string? SvgIcon { get => null; set { } }

#pragma warning restore BL0007
    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the radio group input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string RadioInputGroupClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Gets the resolved CSS class string for the radio input groups field area,
    /// </summary>
    protected string RadioInputGroupFieldClasses { get; private set; } = String.Empty;

    internal TValue? GroupValue => CurrentValue;

    /// <summary>
    /// The unique identifier used to link the group label with the radio container for accessibility.
    /// </summary>
    protected string _labelID = Guid.NewGuid().ToString();

    /// <summary>
    /// Updates the radio input group CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        RadioInputGroupClasses = GetInputClasses(base.AdditionalAttributes);
        RadioInputGroupFieldClasses = GetInputFieldClasses(Orientation);
    }


    /// <summary>
    /// Sets the current value of the radio group. Called by child <see cref="RadioInput{TValue}"/> components.
    /// </summary>
    /// <param name="value">The new value to assign to the group.</param>
    internal void SetGroupValue(TValue value)
     
        =>  CurrentValue = value;
    

    /// <summary>
    /// As radio buttons handle their own selection, this implementation satisfies 
    /// the <see cref="InputBase{TValue}"/> requirement by returning true with a default result.
    /// </summary>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = String.Empty;
        result = default!;
        return true;
    }


    private static string GetInputFieldClasses(Orientation orientation)

        => orientation == Orientation.Horizontal ? GlobalValues.Radio_Input_Group_Field_Area_Class
                                                 : $"{GlobalValues.Radio_Input_Group_Field_Area_Class} {GlobalValues.Radio_Input_Group_Field_Area_Modifier}";


    /// <summary>
    /// Builds the CSS class string for the root element by combining the base input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    private static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Radio_Input_Group_Class} {classData}";
        }

        return @GlobalValues.Radio_Input_Group_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

}
