using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Services;
using BlazorRamp.DebounceFilter.Common.Constants;
using BlazorRamp.DebounceFilter.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorRamp.DebounceFilter.Components;

/// <summary>
/// A debounced filter input component that raises a callback after the user stops typing,
/// optionally validating input against a regular expression and announcing results via an ARIA live region.
/// </summary>
partial class DebounceFilter : IAsyncDisposable
{

    /// <summary>
    /// Gets or sets the name used for the filter input control label.
    /// If the value is null, empty, or whitespace then the field name is used.
    /// </summary>
    [Parameter] public string FilterLabelText { get; set; } = String.Empty;
    /// <summary>
    /// Gets or sets the <c>id</c> attribute applied to the underlying <c>&lt;input&gt;</c>
    /// element. When null, empty, or whitespace a <see cref="Guid"/> string is generated
    /// automatically. Leading and trailing whitespace is trimmed before use.
    /// </summary>
    [Parameter] public string ControlID { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the hint text rendered below the label and above the input field.
    /// When set, the text is normalised so it always ends with a full stop, and is
    /// associated to the input via <c>aria-describedby</c>. When null, empty, or
    /// whitespace no hint element is rendered.
    /// </summary>
    [Parameter] public string HintText { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon alongside the tab title. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Gets or sets the debounce delay in milliseconds used.
    /// Defaults to 250ms/>.
    /// </summary>
    [Parameter] public int DebounceDelayMs { get; set; } = GlobalValues.Debounce_DelayMs;

    /// <summary>
    /// Gets or sets the validation message displayed when the regex pattern is not matched.
    /// </summary>
    [Parameter] public string? ValidationMessage { get; set; }

    /// <summary>
    /// Gets or sets the regular expression pattern used to validate the filter input.
    /// When null or empty no validation is applied.
    /// </summary>
    [Parameter] public string? RegexPattern { get; set; }

    /// <summary>
    /// Gets or sets the error message displayed when the regex pattern fails to compile.
    /// Defaults to "System error, filtering is unavailable at this time." />.
    /// </summary>
    [Parameter] public string? ParseErrorMessage { get; set; } = GlobalValues.Debounce_Filter_Regex_Error_Message;

    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="FilterDataPosition.Start"/>.
    /// </summary>
    [Parameter] public FilterDataPosition FilterDataPosition { get; set; } = FilterDataPosition.Start;

    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked after each debounced input event.
    /// The <see cref="DebouncedFilterResult"/> contains the current filter value, validity state, and any error details.
    /// </summary>
    [Parameter] public Func<DebouncedFilterResult, Task>? OnDebounceFilterResult { get; set; }


    /// <summary>
    /// Gets the <see cref="ElementReference"/> for the underlying <c>&lt;input&gt;</c>
    /// element, available after the component has rendered.
    /// </summary>
    public ElementReference ControlReference  { get; set; }
    private ElementReference MessageElementRef   { get; set; }
    private ElementReference StateIconElementRef { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;
    
    private IJSObjectReference?                    _jSModule        = null;
    private DotNetObjectReference<DebounceFilter>? _dotNetObjectRef = default;

    private int   _debounceDelayMs     = GlobalValues.Debounce_DelayMs;
    private string _filterNameText     = GlobalValues.Debounce_Filter_Label_Text;
    private string _inputID            = String.Empty;
    private string _debounceClasses    = GlobalValues.Debounce_Filter_Class;
    private string _parseErrorMessage = GlobalValues.Debounce_Filter_Regex_Error_Message;
    private string? _regexPattern      = null;
    private string? _validationMessage = GlobalValues.Debounce_Filter_Regex_Validation_Message;
    private string? _hintNormalised    = null;
    private string? _svgVariable       = null;
    private string? _ariaDescribedByID = null;
    private string _hintTextID         = Guid.NewGuid().ToString();

    private bool _disposed = false;

    /// <summary>
    /// Sets derived state from parameters; normalises hint text, resolves the SVG icon variable,
    /// and updates the CSS class string.
    /// </summary>
    protected override void OnParametersSet()
    {
        _hintNormalised  = String.IsNullOrWhiteSpace(HintText) ? null : (HintText.EndsWith('.') ? HintText : HintText.Trim() + ".");
        _ariaDescribedByID = _hintNormalised is null ? null : _hintTextID;
        _svgVariable     = CheckSetSvgVariable(SvgIcon);
        _debounceClasses = GetInputClasses(AdditionalAttributes);
    }

    /// <summary>
    /// Sets one-time state from initial parameter values; resolves the input ID, label text,
    /// regex pattern, validation messages, debounce delay, and aria-describedby association.
    /// </summary>
    protected override void OnInitialized()
    {
        _inputID           = String.IsNullOrWhiteSpace(ControlID) ? Guid.NewGuid().ToString() : ControlID.Trim();
        _filterNameText    = String.IsNullOrWhiteSpace(FilterLabelText) ? GlobalValues.Debounce_Filter_Label_Text : FilterLabelText.Trim();
        _regexPattern      = String.IsNullOrWhiteSpace(RegexPattern) ? null : RegexPattern.Trim();
        _validationMessage = String.IsNullOrWhiteSpace(ValidationMessage) ? GlobalValues.Debounce_Filter_Regex_Validation_Message : ValidationMessage.Trim();
        _parseErrorMessage = String.IsNullOrWhiteSpace(ParseErrorMessage) ? GlobalValues.Debounce_Filter_Regex_Error_Message : ParseErrorMessage.Trim();
        _debounceDelayMs   = DebounceDelayMs < 1 ? GlobalValues.Debounce_DelayMs : DebounceDelayMs;
    }

    /// <summary>
    /// On first render, imports the JavaScript module and registers the debounce input handler,
    /// passing the configuration and .NET callback reference to the JS layer.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (true == firstRender)
        {
            
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Debounce_Filter_File_Path);

            if (_jSModule is not null)
            {
                _dotNetObjectRef = DotNetObjectReference.Create(this);
                var debounceConfig = new DebounceConfiguration<DotNetObjectReference<DebounceFilter>>(_dotNetObjectRef, nameof(HandleDebounceFilterResult),MessageElementRef,StateIconElementRef,
                                                                                                     _debounceDelayMs,_parseErrorMessage,_regexPattern,_validationMessage);

                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Debounce_Filter_Handler,ControlReference,debounceConfig);
            }
               
        }
    
    }

    private static string? CheckSetSvgVariable(string? svgIcon)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{GlobalValues.Debounce_Filter_Svg_Css_Variable_Name}:{iconVariable};";
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base text input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    private static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Debounce_Filter_Class} {classData}";
        }

        return @GlobalValues.Debounce_Filter_Class;
    }


    /// <summary>
    /// Clears the filter input and resets the validation state.
    /// </summary>
    public async Task ClearFilter()
    {
        if (_jSModule is not null && ControlReference.Id is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Clear_Debounce_Filter, ControlReference);
    }

    /// <summary>
    /// Returns a filtered copy of AdditionalAttributes with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

    /// <summary>
    /// Handles the result returned from JavaScript after each debounced input event.
    /// Announces validation messages via the live region service and invokes <see cref="OnDebounceFilterResult"/>.
    /// </summary>
    [JSInvokable]
    public async Task HandleDebounceFilterResult(DebouncedFilterResult filterResult)
    {
        if (OnDebounceFilterResult is not null)
        {
            var hasSystemError = !String.IsNullOrWhiteSpace(filterResult.ExceptionMessage);

            var message = (filterResult.IsValid, hasSystemError) switch
            {
                (true, false) => String.Empty,
                (true, true) => filterResult.ExceptionMessage,
                (false, true) => filterResult.ExceptionMessage,
                (false, false) => _validationMessage,
            };

            if (false == String.IsNullOrWhiteSpace(message))
            {
                AnnouncementType announcementType = hasSystemError ? AnnouncementType.SystemError : AnnouncementType.Info;
                await LiveRegionService.MakeAnnouncement(new(message, announcementType, _filterNameText, LiveRegionType.Polite));
            }

            await OnDebounceFilterResult(filterResult);
        }
    }


    /// <summary>
    /// Performs asynchronous disposal of resources, including the JS module reference and .NET object reference.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        
        _disposed = true;

        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Debounce_Filter_Handler, ControlReference);
                await _jSModule.DisposeAsync();

                _dotNetObjectRef?.Dispose();
            }
            catch { }
        }

    }
}
