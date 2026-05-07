using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components.Summaries;

/// <summary>
/// A cascading container component that wraps form input controls and provides an accessible
/// error summary region. When the <see cref="EditContext"/> raises validation, the summary
/// collects error messages from all registered inputs and renders them as an anchor link list,
/// allowing users to navigate directly to the field containing each error.
/// </summary>
/// <remarks>
/// Place this component inside an <c>&lt;EditForm&gt;</c> and wrap your input controls with it.
/// Each input that inherits from <see cref="InputTypeBase{TValue}"/> automatically registers
/// itself with the summary via the cascading value.
/// <code>
/// &lt;EditForm Model="@_model"&gt;
///     &lt;InputErrorsSummary&gt;
///         &lt;TextInput @bind-Value="_model.Name" /&gt;
///     &lt;/InputErrorsSummary&gt;
/// &lt;/EditForm&gt;
/// </code>
/// </remarks>
public partial class InputErrorsSummary : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the <see cref="EditContext"/> cascaded from the parent <c>&lt;EditForm&gt;</c>.
    /// Used to subscribe to validation events and retrieve validation messages.
    /// </summary>
    [CascadingParameter] private EditContext CurrentEditContext { get; set; } = default!;

    /// <summary>
    /// Gets or sets the child content rendered inside the cascading value scope.
    /// Typically contains the form's input controls.
    /// </summary>
    [Parameter] public RenderFragment?   ChildContent           { get; set; } = default!;

    /// <summary>
    /// Gets or sets the heading text rendered at the top of the error summary section.
    /// When null, empty, or whitespace defaults to "There is a problem with your entries".
    /// </summary>
    [Parameter] public string            Title                  { get; set; } = GlobalValues.Input_Errors_Summary_Title;

    /// <summary>
    /// Gets or sets the suffix appended to each field link in the summary list.
    /// When null, empty, or whitespace defaults to "field"
    /// </summary>
    [Parameter] public string            InputSuffix            { get; set; } = GlobalValues.Input_Errors_Summary_Input_Suffix;

    /// <summary>
    /// Gets or sets when the error summary is displayed. 
    /// <see cref="SummaryDisplay.OnModelValidated"/> shows the summary only after the first
    /// full model validation (i.e. form submit). <see cref="SummaryDisplay.Always"/> shows
    /// it whenever there are errors regardless of whether the form has been submitted.
    /// Defaults to <see cref="SummaryDisplay.OnModelValidated"/>.
    /// </summary>
    [Parameter] public SummaryDisplay    SummaryDisplay         { get; set; } = SummaryDisplay.OnModelValidated;

    /// <summary>
    /// Gets or sets the heading level used for the error summary title.
    /// Allows the heading to fit correctly within the page's heading hierarchy.
    /// Defaults to <see cref="TitleHeadingLevel.H2"/>.
    /// </summary>
    [Parameter] public TitleHeadingLevel TitleHeadingLevel      { get; set; } = TitleHeadingLevel.H2;

    /// <summary>
    /// Gets or sets the JavaScript runtime used to load the inputs JS module
    /// and invoke focus management functions.
    /// </summary>
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;

    private string _summarySectionID = Guid.NewGuid().ToString();
    private string _headingID        = Guid.NewGuid().ToString();
    private string _headingTitle     = GlobalValues.Input_Errors_Summary_Title;
    private bool   _modelValidated   = false;
    private string _inputSuffix      = GlobalValues.Input_Errors_Summary_Input_Suffix;

    private readonly Dictionary<FieldIdentifier, InputMapItem> _inputMap    = [];
    private readonly List<ErrorSummaryItem>                   _summaryItems = [];

    /// <summary>
    /// Updates <see cref="_headingTitle"/> and <see cref="_inputSuffix"/> from parameters
    /// on each render cycle.
    /// </summary>
    protected override void OnParametersSet()
    {
        _headingTitle = String.IsNullOrWhiteSpace(Title) ? GlobalValues.Input_Errors_Summary_Title : Title.Trim();
        _inputSuffix  = String.IsNullOrWhiteSpace(InputSuffix) ? GlobalValues.Input_Errors_Summary_Input_Suffix : InputSuffix.Trim();
    }

    /// <summary>
    /// Subscribes to <see cref="EditContext.OnValidationRequested"/> and
    /// <see cref="EditContext.OnValidationStateChanged"/> on the cascaded <see cref="EditContext"/>
    /// to keep the summary in sync with the form's validation state.
    /// </summary>
    protected override void OnInitialized()
    {
        if(CurrentEditContext is not null)
        {
            CurrentEditContext.OnValidationRequested += CurrentEditContext_OnValidationRequested;
            CurrentEditContext.OnValidationStateChanged += CurrentEditContext_OnValidationStateChanged;
        }
     
    }

    /// <summary>
    /// Loads the JavaScript module on first render so focus management functions
    /// are available for subsequent validation events.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);
    }


    private async Task FocusInput(string controlID)
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Set_Input_Focus, controlID);
    }

    private void CurrentEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
    {
        _summaryItems.Clear();
        _summaryItems.AddRange(BuildSummaryList(_inputMap, CurrentEditContext));
    }

    private async void CurrentEditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        try
        {
            _modelValidated = true;
            _summaryItems.Clear();
            _summaryItems.AddRange(BuildSummaryList(_inputMap, CurrentEditContext));

            if (_summaryItems.Count > 0 && _jSModule is not null)
            {
                await Task.Yield();
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Set_Summary_Focus, _summarySectionID);
            }
        }
        catch { }


    }

    internal void AddToInputMap(FieldIdentifier fieldIdentifier, string displayName, string controlID)
    {
        if (false == _inputMap.ContainsKey(fieldIdentifier)) _inputMap[fieldIdentifier] = new(displayName,controlID);
    }

    private List<ErrorSummaryItem> BuildSummaryList(Dictionary<FieldIdentifier, InputMapItem> inputMap, EditContext currentContext)
    {
        List<ErrorSummaryItem> summaryList = [];
        
        foreach(var key in inputMap.Keys)
        {
            var errorList = currentContext.GetValidationMessages(key).ToList();
            if (errorList.Count > 0)
            {
                var inputMapItem = inputMap[key];
                summaryList.Add(new ErrorSummaryItem(errorList.ToList(), inputMapItem.DisplayName, inputMapItem.ControlID));
            }
        }

        return summaryList;
    }

    private string GetSectionClasses(int errorCount, SummaryDisplay summaryDisplay, bool modelValidated)
    
        => (errorCount, summaryDisplay, modelValidated) switch
        {
            _ when errorCount > 0 && summaryDisplay == SummaryDisplay.Always                        
                => GlobalValues.Input_Errors_Summary_Class,
            _ when errorCount > 0 && summaryDisplay == SummaryDisplay.OnModelValidated && modelValidated    
                => GlobalValues.Input_Errors_Summary_Class,
            _   => $"{GlobalValues.Input_Errors_Summary_Class} {GlobalValues.Input_Errors_Summary_No_Errors_Modifier}"
        };




    /// <summary>
    /// Unsubscribes from <see cref="EditContext"/> validation events and disposes the
    /// JavaScript module reference to prevent memory leaks when the component is removed
    /// from the render tree.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (CurrentEditContext is not null)
            {
                CurrentEditContext.OnValidationRequested -= CurrentEditContext_OnValidationRequested;
                CurrentEditContext.OnValidationStateChanged -= CurrentEditContext_OnValidationStateChanged;
            }
            if (_jSModule is not null) await _jSModule.DisposeAsync();
        }
        catch { }
    }
}
