using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components;

public class TextAreaTypeInput : InputTypeBase<string>
{

    /// <summary>
    /// Gets or sets whether the input value is updated on every keystroke via the
    /// <c>oninput</c> event. When <c>false</c> the value updates on <c>onchange</c>
    /// (i.e. when the field loses focus). Defaults to <c>false</c>.
    /// </summary>
    [Parameter] public bool UpdateOnInput { get; set; } = false;

    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.Start"/>.
    /// </summary>
    [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.Start;

    /// <summary>
    /// Gets or sets whether the value should be trimmed during the on blur event. Defaults to <c>false</c>.
    /// </summary>
    [Parameter] public bool TrimOnBlur { get; set; } = false;

    [Parameter] public int UpdateOnInputDelayMs  { get; set; } = GlobalValues.TextArea_Debounce_Default;

    [Parameter] public int TextAreaRows         { get; set; } = GlobalValues.TextArea_Rows_Default;

    [Parameter] public bool AutoSize            { get; set; } = true;

    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the textarea input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string TextAreaInputClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Gets the resolved CSS class string applied to the textarea input,
    /// </summary>
    protected string TextAreaClasses { get; private set; } = GlobalValues.TextArea_Input_Field_Class;
    protected int InputRows { get; private set; } = GlobalValues.TextArea_Rows_Default;

    protected string? TextAreaValue { get; private set; }

    private CancellationTokenSource _debounceTokenSource = default!;
    private int?                    _debounceDelayMs     = 500;

    /// <summary>
    /// Updates the text input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        TextAreaInputClasses = GetInputClasses(base.AdditionalAttributes);
        TextAreaClasses      = GetTextAreaClasses(AutoSize);
        InputRows            = TextAreaRows < 1 ? GlobalValues.TextArea_Rows_Default : TextAreaRows;

        _debounceDelayMs = UpdateOnInputDelayMs < GlobalValues.TextArea_Debounce_Default ? GlobalValues.TextArea_Debounce_Default : UpdateOnInputDelayMs;
    }

    protected override void OnInitialized()
    {
        _debounceTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// Always returns <c>true</c> passing the raw string value through unchanged.
    /// Validation is handled entirely by the consuming application's validator.
    /// </summary>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string? validationErrorMessage)
    {

        validationErrorMessage = String.Empty;

        result = value!;

        return true;
    }


    /// <summary>
    /// Handles the binding set event, updating <see cref="InputBase{TValue}.CurrentValueAsString"/>
    /// with the raw input value. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/>
    /// is <c>true</c>.
    /// </summary>
    protected async Task HandlePropertySet(string? value)
    {
        if (base.IsDisabled) return;

        TextAreaValue = value;

        if (false == UpdateOnInput)
        {
            CurrentValueAsString = value;
            return;
        }
        
        TextAreaValue = value;

        _debounceTokenSource?.Cancel();
        _debounceTokenSource?.Dispose();
        _debounceTokenSource = new CancellationTokenSource();
             

        await UpdateCurrentValueAsString(value, 3000, _debounceTokenSource.Token);
    }


    /// <summary>
    /// Handles the input blur event and trims the whitespace from the <see cref="InputBase{TValue}.CurrentValue"/> 
    /// if the option has been set for this <see cref="TrimOnBlur" />
    /// </summary>
    protected async Task HandleOnBlur()
    {
        _debounceTokenSource?.Cancel();

        if (CurrentValueAsString is not null && true == TrimOnBlur) CurrentValueAsString = CurrentValueAsString.Trim();
    }


    private async Task UpdateCurrentValueAsString(string? value, int timeToWait, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(timeToWait, cancellationToken);

            CurrentValueAsString = value;
            await InvokeAsync(StateHasChanged);
        }
        catch (TaskCanceledException) { }//nothing to do.
    }


    private static string GetTextAreaClasses(bool autosize)

        => autosize ? $"{GlobalValues.TextArea_Input_Field_Class} {GlobalValues.TextArea_Input_Field_Autosize_Modifier}" : GlobalValues.TextArea_Input_Field_Class;
    

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base text input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    private static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.TextArea_Input_Class} {classData}";
        }

        return @GlobalValues.TextArea_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the textarea input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _debounceTokenSource?.Cancel();
            _debounceTokenSource?.Dispose();
        }
        base.Dispose(disposing);
    }
}
