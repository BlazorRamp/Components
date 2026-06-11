using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components;

/// <summary>
/// Renders an accessible multi-line text area input with optional character counting,
/// screen reader announcements, and debounced value updates. Inherits validation state
/// management, hint text, aria-disabled support, and SVG icon support from
/// <see cref="InputTypeBase{TValue}"/>. The character counter is driven via JavaScript
/// interop and updates directly in the DOM without a Blazor re-render. Screen reader
/// announcements are debounced to avoid overwhelming assistive technology during rapid
/// typing, and are triggered immediately on paste or drag-and-drop operations.
/// </summary>
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

    /// <summary>
    /// Gets or sets the debounce delay in milliseconds applied when <see cref="UpdateOnInput"/> is
    /// <c>true</c>. The value update is deferred by this duration after the last keystroke before
    /// <see cref="InputBase{TValue}.CurrentValueAsString"/> is updated. Values below
    /// <see cref="GlobalValues.TextArea_Debounce_Default"/> are clamped to the default.
    /// Defaults to <see cref="GlobalValues.TextArea_Debounce_Default"/>.
    /// </summary>
    [Parameter] public int UpdateOnInputDelayMs  { get; set; } = GlobalValues.TextArea_Debounce_Default;

    /// <summary>
    /// Gets or sets the number of visible text rows rendered for the textarea element.
    /// Values less than <c>1</c> fall back to the default of 5.
    /// </summary>
    [Parameter] public int TextAreaRows         { get; set; } = GlobalValues.TextArea_Rows_Default;

    /// <summary>
    /// Gets or sets whether the textarea grows vertically to fit its content using the
    /// CSS <c>field-sizing: content</c> property. Defaults to <c>true</c>.
    /// </summary>
    [Parameter] public bool AutoSize            { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of characters for the textarea counter
    /// and screen reader announcements. This does not restrict the actual number of characters
    /// that can be entered. Values less than 1 fallback to the default of 8000.
    /// </summary>
    [Parameter] public int    MaxCharacters           { get; set; } = GlobalValues.TextArea_Max_Characters;

    /// <summary>
    /// Gets or sets the template string displayed in the character counter when the user
    /// is within the character limit. Must contain the <c>{count}</c> token, which is
    /// replaced with the number of characters remaining. When null, empty, whitespace, or
    /// missing the token, falls back to the default of: "You have {count} characters remaining."/>.
    /// </summary>
    [Parameter] public string CharactersRemainingText { get; set; } = GlobalValues.TextArea_Characters_Remaining_Text;

    /// <summary>
    /// Gets or sets the template string displayed in the character counter when the user
    /// has exceeded the character limit. Must contain the <c>{count}</c> token, which is
    /// replaced with the number of characters over the limit. When null, empty, whitespace,
    /// or missing the token, falls back to "You are {count} characters over the limit" />.
    /// </summary>
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

    /// <summary>
    /// Gets the resolved row count applied to the <c>rows</c> attribute of the
    /// <c>&lt;textarea&gt;</c> element. When <see cref="TextAreaRows"/> is less than
    /// <c>1</c> this returns <see cref="GlobalValues.TextArea_Rows_Default"/>.
    /// </summary>
    protected int InputRows { get; private set; } = GlobalValues.TextArea_Rows_Default;

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the textarea element,
    /// managed independently of <see cref="InputBase{TValue}.CurrentValueAsString"/> to
    /// preserve mid-entry display state and support debounced updates when
    /// <see cref="UpdateOnInput"/> is <c>true</c>.
    /// </summary>
    protected string? TextAreaValue { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the character counter
    /// <c>&lt;span&gt;</c> element, available after the component has rendered. Passed
    /// to the JavaScript character-count handler so the counter text can be updated
    /// directly in the DOM without a Blazor re-render.
    /// </summary>
    protected ElementReference MessageSpanRef { get; set; } = default!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;

    private DotNetObjectReference<TextAreaTypeInput>? _dotNetObjectRef;

    private IJSObjectReference? _jSModule = null;

    private CancellationTokenSource _inputDebounceTS         = default!;
    private CancellationTokenSource _announceDebounceTS      = default!;
    private int                     _debounceDelayMs         = 500;
    private int                     _announceDebounceDelayMs = 1000;
    
    private string                  _remainingText       = GlobalValues.TextArea_Characters_Remaining_Text;
    private string                  _overlimitText       = GlobalValues.TextArea_Characters_Over_Limit_Text;
    private int                     _maxCharacters       = GlobalValues.TextArea_Max_Characters;
    private int                     _liveCharacterCount  = 0;


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

    /// <summary>
    /// Resolves character count configuration from parameters and initialises
    /// <see cref="TextAreaValue"/> and the live character count from the current
    /// <see cref="InputBase{TValue}.Value"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _announceDebounceTS = new CancellationTokenSource();
        _inputDebounceTS    = new CancellationTokenSource();

        _maxCharacters       = MaxCharacters < 1                                  ? GlobalValues.TextArea_Max_Characters             : MaxCharacters;
        _remainingText       = String.IsNullOrWhiteSpace(CharactersRemainingText) || !CharactersRemainingText.Contains("{count}") 
                                    ? GlobalValues.TextArea_Characters_Remaining_Text  : CharactersRemainingText.Trim();

        _overlimitText       = String.IsNullOrWhiteSpace(CharactersOverLimitText) || !CharactersOverLimitText.Contains("{count}")
                                    ? GlobalValues.TextArea_Characters_Over_Limit_Text : CharactersOverLimitText.Trim();

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
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the update is
    /// deferred by <see cref="UpdateOnInputDelayMs"/> milliseconds via a debounce cancellation token.
    /// Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
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

    /// <summary>
    /// Handles the <c>keydown</c> event on the textarea. Cancels any pending announcement
    /// and schedules a debounced screen reader character-count announcement via
    /// <see cref="AnnounceCharacterCount"/>.
    /// </summary>
    protected async Task HandleKeyDown(KeyboardEventArgs e)
    {
        _announceDebounceTS?.Cancel();
        _announceDebounceTS?.Dispose();
        _announceDebounceTS = new CancellationTokenSource();

        await AnnounceCharacterCount(_remainingText, _overlimitText, _announceDebounceDelayMs, _maxCharacters, _announceDebounceTS.Token);

       
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


    /// <summary>
    /// Schedules a debounced screen reader announcement of the current character count.
    /// Waits for <paramref name="timeToWait"/> milliseconds before announcing. If the
    /// cancellation token is cancelled during the wait the announcement is silently
    /// discarded. Additionally suppresses announcements that occur within
    /// avoid overwhelming screen reader users during rapid typing.
    /// </summary>
    /// <param name="remainingText">The template string for within-limit announcements.</param>
    /// <param name="overlimitText">The template string for over-limit announcements.</param>
    /// <param name="timeToWait">The debounce delay in milliseconds.</param>
    /// <param name="maxLength">The configured maximum character count.</param>
    /// <param name="cancellationToken">Token used to cancel the pending announcement.</param>
    protected async Task AnnounceCharacterCount(string remainingText, string overlimitText, int timeToWait, int maxLength, CancellationToken cancellationToken)
    {
        try
        {
                
            await Task.Delay(timeToWait, cancellationToken);

            await MakeAnnouncement();

        }
        catch (TaskCanceledException) { }//nothing to do
    }

    private async Task MakeAnnouncement()
    {
        var countValue      = Math.Abs(_maxCharacters - _liveCharacterCount);
        var messageTemplate = _liveCharacterCount <= _maxCharacters ? _remainingText : _overlimitText;
        var message         = messageTemplate.Replace("{count}", countValue.ToString());
        var announcement    = new Announcement(message, AnnouncementType.Info, $"{LabelNameText}", LiveRegionType.Polite);

        await LiveRegionService.MakeAnnouncement(announcement,false);
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


    /// <summary>
    /// Invoked via JavaScript interop when the textarea input event fires. Updates the
    /// internal live character count used for screen reader announcements, and immediately
    /// triggers an announcement when the change originated from a paste or drag-and-drop
    /// operation.
    /// </summary>
    /// <param name="currentCount">The current character count of the textarea value.</param>
    /// <param name="pastedText">
    /// <c>true</c> when the input event type was <c>insertFromPaste</c> or
    /// <c>insertFromDrop</c>; otherwise <c>false</c>.
    /// </param>
    [JSInvokable]
    public async Task UpdateLiveCharacterCount(int currentCount, bool pastedText)
    {
        _liveCharacterCount = currentCount;

        if (pastedText) await MakeAnnouncement();
    }

    /// <summary>
    /// Cancels and disposes active debounce cancellation token sources to ensure
    /// no pending delayed tasks run after the component is removed from the render tree.
    /// </summary>
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
    /// Unregisters the textarea character count JavaScript handlers, disposes the JavaScript
    /// module reference and <see cref="DotNetObjectReference{T}"/>, and calls
    /// <see cref="InputTypeBase{TValue}.DisposeAsync"/> to release base resources.
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

