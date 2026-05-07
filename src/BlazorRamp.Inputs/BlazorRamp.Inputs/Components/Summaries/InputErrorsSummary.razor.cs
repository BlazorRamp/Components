using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components.Summaries;

public partial class InputErrorsSummary : IAsyncDisposable
{
    [CascadingParameter] private EditContext CurrentEditContext { get; set; } = default!;
    [Parameter] public RenderFragment?   ChildContent           { get; set; } = default!;
    [Parameter] public string            Title                  { get; set; } = GlobalValues.Input_Errors_Summary_Title;
    [Parameter] public string            InputSuffix            { get; set; } = GlobalValues.Input_Errors_Summary_Input_Suffix;
    [Parameter] public SummaryDisplay    SummaryDisplay         { get; set; } = SummaryDisplay.OnModelValidated;
    [Parameter] public TitleHeadingLevel TitleHeadingLevel      { get; set; } = TitleHeadingLevel.H2;


    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;

    private string _summarySectionID = Guid.NewGuid().ToString();
    private string _headingID        = Guid.NewGuid().ToString();
    private string _headingTitle     = GlobalValues.Input_Errors_Summary_Title;
    private bool   _modelValidated   = false;
    private string _inputSuffix      = GlobalValues.Input_Errors_Summary_Input_Suffix;

    private readonly Dictionary<FieldIdentifier, InputMapItem> _inputMap    = [];
    private readonly List<ErrorSummaryItem>                   _summaryItems = [];

    protected override void OnParametersSet()
    {
        _headingTitle = String.IsNullOrWhiteSpace(Title) ? GlobalValues.Input_Errors_Summary_Title : Title.Trim();
        _inputSuffix  = String.IsNullOrWhiteSpace(InputSuffix) ? GlobalValues.Input_Errors_Summary_Input_Suffix : InputSuffix.Trim();
    }

    protected override void OnInitialized()
    {
        if(CurrentEditContext is not null)
        {
            CurrentEditContext.OnValidationRequested += CurrentEditContext_OnValidationRequested;
            CurrentEditContext.OnValidationStateChanged += CurrentEditContext_OnValidationStateChanged;
        }
     
    }

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
