using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

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

    [Parameter] public int    MaxCharacters           { get; set; } = GlobalValues.TextArea_Max_Characters;
    [Parameter] public string CharactersRemainingText { get; set; } = GlobalValues.TextArea_Characters_Remaining_Text;
    [Parameter] public string CharactersOverLimitText { get; set; } = GlobalValues.TextArea_Characters_Over_Limit_Text;

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

    protected ElementReference MessageSpanRef { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;

    private DotNetObjectReference<TextAreaTypeInput>? _dotNetObjectRef;

    private IJSObjectReference? _jSModule = null;

    private CancellationTokenSource _inputDebounceTS     = default!;
    private CancellationTokenSource _announceDebounceTS  = default!;
    private int                     _debounceDelayMs     = 500;
    private string                  _remainingText       = GlobalValues.TextArea_Characters_Remaining_Text;
    private string                  _overlimitText       = GlobalValues.TextArea_Characters_Over_Limit_Text;
    private int                     _maxCharacters       = GlobalValues.TextArea_Max_Characters;
    private int                     _liveCharacterCount  = 0;
    private DateTime                _lastAnnouncedAt     = DateTime.MinValue;
    private const int               AnnounceDebounceMs   = 1000;     
    private const int               AnnounceDelaySeconds = 2;


    /// <summary>
    /// Updates the text input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        TextAreaInputClasses = GetInputClasses(base.AdditionalAttributes);
        TextAreaClasses      = GetTextAreaClasses(AutoSize);
        InputRows            = TextAreaRows < 1 ? GlobalValues.TextArea_Rows_Default : TextAreaRows;

        _debounceDelayMs     = UpdateOnInputDelayMs < GlobalValues.TextArea_Debounce_Default ? GlobalValues.TextArea_Debounce_Default : UpdateOnInputDelayMs;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _announceDebounceTS = new CancellationTokenSource();
        _inputDebounceTS    = new CancellationTokenSource();

        _maxCharacters       = MaxCharacters < 1                                  ? GlobalValues.TextArea_Max_Characters             : MaxCharacters;
        _remainingText       = String.IsNullOrWhiteSpace(CharactersRemainingText) ? GlobalValues.TextArea_Characters_Remaining_Text  : CharactersRemainingText.Trim();
        _overlimitText       = String.IsNullOrWhiteSpace(CharactersOverLimitText) ? GlobalValues.TextArea_Characters_Over_Limit_Text : CharactersOverLimitText.Trim();
        _liveCharacterCount  = Value?.Length ?? 0; 

        TextAreaValue = Value;
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
    /// Loads the JavaScript module on first render and registers the character count handling and the 
    /// callback used to get the current value for screen reader announcements as the event model could be 
    ///  onchange and not oninput which is needed.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);

            if (_jSModule is not null)
            {
                _dotNetObjectRef = DotNetObjectReference.Create(this);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Count_Callback_Handlers, _dotNetObjectRef, nameof(UpdateLiveCharacterCount),
                                                base.ControlReference, MessageSpanRef, _remainingText, _overlimitText, GlobalValues.TextArea_Input_Field_Counter_Modifier, _maxCharacters);
            }

        }
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

        _inputDebounceTS?.Cancel();
        _inputDebounceTS?.Dispose();
        _inputDebounceTS = new CancellationTokenSource();
             

        await UpdateCurrentValueAsString(TextAreaValue, _debounceDelayMs, _inputDebounceTS.Token);
    }


    /// <summary>
    /// Handles the input blur event and trims the whitespace from the <see cref="InputBase{TValue}.CurrentValue"/> 
    /// if the option has been set for this <see cref="TrimOnBlur" />
    /// </summary>
    protected async Task HandleOnBlur()
    {
        _announceDebounceTS?.Cancel();

        if (CurrentValueAsString is not null && true == TrimOnBlur) CurrentValueAsString = CurrentValueAsString.Trim();
    }
    protected async Task HandleKeyDown(KeyboardEventArgs e)
    {
        _announceDebounceTS?.Cancel();
        _announceDebounceTS?.Dispose();
        _announceDebounceTS = new CancellationTokenSource();

        await AnnounceCharacterCount(_remainingText, _overlimitText, AnnounceDebounceMs, _liveCharacterCount, _maxCharacters, AnnounceDelaySeconds, _lastAnnouncedAt, _announceDebounceTS.Token);

        _lastAnnouncedAt = DateTime.Now;
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

    protected async Task AnnounceCharacterCount(string remainingText, string overlimitText, int timeToWait, int currentLength, int maxLength, 
                                               int announceDelaySeconds, DateTime lastDateTime, CancellationToken cancellationToken)
    {
        try
        {
            if ((currentLength * 100.00 / maxLength) < 70) return;
                
            await Task.Delay(timeToWait, cancellationToken);

            var timeSpan = DateTime.Now.Subtract(lastDateTime);

            //if (timeSpan.Seconds < announceDelaySeconds) return;

            var countValue      = Math.Abs(maxLength - currentLength);
            var messageTemplate = currentLength <= maxLength ? remainingText : overlimitText;
            var message         = messageTemplate.Replace("{count}", countValue.ToString());
            var announcement    = new Announcement(message, AnnouncementType.Info, $"{LabelNameText}", LiveRegionType.Polite);

            await LiveRegionService.MakeAnnouncement(announcement);

        }
        catch (TaskCanceledException) { }//nothing to do
    }

    private static string GetCurrentCountString(string currentCountText, int currentLength, int maxLength)

        => $"{currentCountText} {currentLength} / {maxLength}";

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



    [JSInvokable]
    public async Task UpdateLiveCharacterCount(int currentCount)
    
        => _liveCharacterCount = currentCount;
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _inputDebounceTS?.Cancel();
            _inputDebounceTS?.Dispose();
            _announceDebounceTS?.Cancel(); 
            _announceDebounceTS?.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Unregisters the date input JavaScript handlers, disposes the JavaScript module reference,
    /// and calls <see cref="InputTypeBase{TValue}.DisposeAsync"/> to release base resources.
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Count_Callback_Handlers, base.ControlReference);
                await _jSModule.DisposeAsync();

                _dotNetObjectRef?.Dispose();
            }
            catch { }
        }
        await base.DisposeAsync();
    }
}

