using BlazorRamp.DebounceFilter.Common.Constants;
using BlazorRamp.DebounceFilter.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DebounceFilter.Components;

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
    [Parameter] public string? ValidationMessage { get; set; }

    [Parameter] public string? RegexPattern { get; set; }

    [Parameter] public string? ParseErrorMessage { get; set; } = GlobalValues.Debounce_Filter_Regex_Error_Message;
    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="FilterDataPosition.Start"/>.
    /// </summary>
    [Parameter] public FilterDataPosition FilterDataPosition { get; set; } = FilterDataPosition.Start;

    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter] public Func<DebouncedFilterResult, Task>? OnDebounceFilterResult { get; set; }
    private ElementReference InputRef            { get; set; }
    private ElementReference MessageElementRef   { get; set; }
    private ElementReference StateIconElementRef { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    
    
    
    private IJSObjectReference?                    _jSModule        = null;
    private DotNetObjectReference<DebounceFilter>? _dotNetObjectRef = default;

    private int   _debounceDelayMs     = GlobalValues.Debounce_DelayMs;
    private string _filterNameText     = GlobalValues.Debounce_Filter_Label_Text;
    private string _inputID            = String.Empty;
    private string _debounceClasses    = GlobalValues.Debounce_Filter_Class;
    private string _parseErrorMessage = GlobalValues.Debounce_Filter_Regex_Error_Message;
    private string? _regexPattern      = null;
    private string? _validationMessage = null;
    private string? _hintNormalised    = null;
    private string? _svgVariable       = null;
    private string? _ariaDescribedByID = null;
    private string _hintTextID         = Guid.NewGuid().ToString();



    protected override void OnParametersSet()
    {
        _hintNormalised  = String.IsNullOrWhiteSpace(HintText) ? null : (HintText.EndsWith('.') ? HintText : HintText.Trim() + ".");
        _svgVariable     = CheckSetSvgVariable(SvgIcon);
        _debounceClasses = GetInputClasses(AdditionalAttributes);
    }
    protected override void OnInitialized()
    {
        _inputID           = String.IsNullOrWhiteSpace(ControlID) ? Guid.NewGuid().ToString() : ControlID.Trim();
        _filterNameText    = String.IsNullOrWhiteSpace(FilterLabelText) ? GlobalValues.Debounce_Filter_Label_Text : FilterLabelText.Trim();
        _regexPattern      = String.IsNullOrWhiteSpace(RegexPattern) ? null : RegexPattern.Trim();
        _validationMessage = String.IsNullOrWhiteSpace(ValidationMessage) ? null : ValidationMessage.Trim();
        _parseErrorMessage = String.IsNullOrWhiteSpace(ParseErrorMessage) ? GlobalValues.Debounce_Filter_Regex_Error_Message : ParseErrorMessage.Trim();
        _debounceDelayMs   = DebounceDelayMs < 1 ? GlobalValues.Debounce_DelayMs : DebounceDelayMs;
        _ariaDescribedByID = _hintTextID;


    }

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

                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Debounce_Filter_Handler,InputRef,debounceConfig);
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



    public async Task ClearFilter()
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Clear_Debounce_Filter, InputRef);
    }

    /// <summary>
    /// Returns a filtered copy of AdditionalAttributes with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();



    [JSInvokable]
    public async Task HandleDebounceFilterResult(DebouncedFilterResult filterResult)
    {
        if (OnDebounceFilterResult is not null) await OnDebounceFilterResult(filterResult);
    }
       


    public async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Debounce_Filter_Handler, InputRef);
                await _jSModule.DisposeAsync();

                _dotNetObjectRef?.Dispose();
            }
            catch { }
        }

    }
}
