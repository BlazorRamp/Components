using BlazorRamp.Accordion.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.Accordion.Components;

public partial class AccordionItem : IDisposable
{
    [CascadingParameter] public Accordion ParentControl { get; set; } = default!;

    [Parameter]                 public RenderFragment PanelContent  { get; set; } = default!;
    [Parameter, EditorRequired] public string         HeadingText   { get; set; } = default!;
    [Parameter]                 public bool           PanelIsRegion { get; set; } = false;
    [Parameter]                 public bool           Expanded      { get; set; } = false;

    [Parameter] public EventCallback<bool> ExpandedChanged { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether <c>tabindex="0"</c> is applied to the accordion panel,
    /// enabling keyboard focus and arrow key scrolling if the panel has a constrained height.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool PanelHasTabIndex { get; set; } = true;
    /// <summary>
    /// Gets or sets a value indicating whether the tab panel content is retained in the DOM
    /// when the tab is not active. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool PersistContent { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon alongside the heading title. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;
    private ElementReference AccordionButtonRef { get; set; }

    internal bool  IsExpanded         { get; private set; } = false;
    private string AccordionPanelID   { get; } = $"accordion-panel-{Guid.NewGuid()}";
    private string AccordionButtonID  { get; } = $"accordion-button-{Guid.NewGuid()}";
    private string AccordionHeadingID { get; } = $"accordion-heading-{Guid.NewGuid()}";
    internal Guid  AccordionItemID    { get; } = Guid.NewGuid();

    private string  _headingText    = String.Empty;
    private string? _svgVariable    = null;
    private bool    _codeFocused    = false;
    private bool    _persistContent = true;
    private bool    _performRender  = false;
    protected override void OnParametersSet()
    {
        _svgVariable = CheckSetSvgVariable(SvgIcon);
        _headingText = (false == String.IsNullOrWhiteSpace(HeadingText)) ? HeadingText.Trim() : throw new ArgumentNullException(nameof(HeadingText), GlobalValues.Heading_Text_Exception_Message);

        _persistContent = PersistContent;
        _performRender  = this.IsExpanded;

     
    }

    protected override void OnInitialized()
    {
         ParentControl?.AddAccordionItem(this);

        IsExpanded = Expanded;
    }


    private void HandleHeaderFocusIn()

        => ParentControl?.NotifyHeadingFocusIn(AccordionItemID);

    private void HandleHeaderFocusOut()
    {
        _codeFocused = false;
        ParentControl?.NotifyHeadingFocusOut();
    }

    internal async ValueTask SetFocus(bool codeFocused = false)
    {   
        _codeFocused = codeFocused;
        await AccordionButtonRef.FocusAsync();
    }

    internal async Task ToggleExpandedState()
    
        => await RaiseExpandedChanged(!IsExpanded);
    
    private async Task RaiseHeadingClicked()
        
        => await ParentControl!.NotifyHeadingClicked(AccordionItemID);
    
    private async Task RaiseExpandedChanged(bool expanded)
    {
        if (IsExpanded == expanded) return;

        IsExpanded = expanded;

        if (ExpandedChanged.HasDelegate)
            await ExpandedChanged.InvokeAsync(expanded);
        else
            await InvokeAsync(StateHasChanged);

    }

    private string GetTriggerClasses(bool codeFocused)
     
        => $"{GlobalValues.Accordion_Trigger_Class}{(codeFocused ? " " + GlobalValues.Accordion_Trigger_Focus_Modifier : "")}";
        
    

    /// <summary>
    /// Validates the <see cref="SvgIcon"/> parameter and returns a CSS inline style string
    /// setting the internal SVG custom property, or <see langword="null"/> if the value is
    /// absent or does not begin with <c>--</c>.
    /// </summary>
    private string? CheckSetSvgVariable(string? svgIcon)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{GlobalValues.Accordion_Svg_Css_Variable_Name}:{iconVariable};";
    }

    public void Dispose()
    
        => ParentControl?.RemoveAccordionItem(this);    
    
}
