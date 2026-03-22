using BlazorRamp.Accordion.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.Accordion.Components;

/// <summary>
/// A Blazor component that renders an individual accordion item, consisting of a
/// semantic heading with a trigger button and a collapsible content panel.
/// Must be used as a direct child of <see cref="Accordion"/>.
/// </summary>
public partial class AccordionItem : IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="Accordion"/> component. Populated automatically
    /// via cascading value.
    /// </summary>
    [CascadingParameter] public Accordion ParentControl { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content rendered inside the accordion panel.
    /// </summary>
    [Parameter]                 public RenderFragment PanelContent  { get; set; } = default!;

    /// <summary>
    /// Gets or sets the visible label for the accordion trigger button. Also used as the
    /// accessible name. This parameter is required and cannot be null, empty, or whitespace.
    /// </summary>
    [Parameter, EditorRequired] public string         HeadingText   { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the panel element is given
    /// <c>role="region"</c>, making it a landmark navigable by screen readers.
    /// Only set to <see langword="true"/> when the panel contains sufficiently important
    /// content to warrant landmark navigation. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]                 public bool           PanelIsRegion { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the panel is expanded on initial render.
    /// Supports two-way binding via <see cref="ExpandedChanged"/>. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]                 public bool           Expanded      { get; set; } = false;

    /// <summary>
    /// Gets or sets the callback invoked when the expanded state changes, supporting
    /// two-way binding with <c>@bind-Expanded</c>.
    /// </summary>
    [Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether <c>tabindex="0"</c> is applied to the
    /// accordion panel, enabling keyboard focus and arrow key scrolling if the panel
    /// has a constrained height. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool PanelHasTabIndex { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the panel content is retained in the DOM
    /// when the panel is collapsed. Defaults to <see langword="true"/>.
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


    /// <summary>
    /// Validates parameters on each render cycle, throwing if <see cref="HeadingText"/>
    /// is null, empty, or whitespace, and resolving the SVG icon variable and persist
    /// content state.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="HeadingText"/> is null, empty, or whitespace.
    /// </exception>
    protected override void OnParametersSet()
    {
        _svgVariable = CheckSetSvgVariable(SvgIcon);
        _headingText = (false == String.IsNullOrWhiteSpace(HeadingText)) ? HeadingText.Trim() : throw new ArgumentNullException(nameof(HeadingText), GlobalValues.Heading_Text_Exception_Message);

        _persistContent = PersistContent;
    
    }

    /// <summary>
    /// Registers this item with the parent <see cref="Accordion"/> component and sets
    /// the initial expanded state from the <see cref="Expanded"/> parameter.
    /// </summary>
    protected override void OnInitialized()
    {
         ParentControl?.AddAccordionItem(this);

        IsExpanded = Expanded;
    }

    /// <summary>
    /// Notifies the parent <see cref="Accordion"/> that this item's trigger has received focus.
    /// </summary>
    private void HandleHeaderFocusIn()

        => ParentControl?.NotifyHeadingFocusIn(AccordionItemID);

    /// <summary>
    /// Clears the code-focused state and notifies the parent <see cref="Accordion"/> that
    /// this item's trigger has lost focus.
    /// </summary>
    private void HandleHeaderFocusOut()
    {
        _codeFocused = false;
        ParentControl?.NotifyHeadingFocusOut();
    }

    /// <summary>
    /// Programmatically focuses the trigger button, optionally applying the code-focused
    /// visual indicator.
    /// </summary>
    /// <param name="codeFocused">
    /// When <see langword="true"/>, applies the code-focused modifier class to the trigger.
    /// Defaults to <see langword="false"/>.
    /// </param>
    internal async ValueTask SetFocus(bool codeFocused = false)
    {   
        _codeFocused = codeFocused;
        await AccordionButtonRef.FocusAsync();
    }

    /// <summary>
    /// Toggles the expanded state of this accordion item.
    /// </summary>
    internal async Task ToggleExpandedState()
    
        => await RaiseExpandedChanged(!IsExpanded);

    /// <summary>
    /// Notifies the parent <see cref="Accordion"/> that this item's heading was clicked.
    /// </summary>
    private async Task RaiseHeadingClicked()
        
        => await ParentControl!.NotifyHeadingClicked(AccordionItemID);

    /// <summary>
    /// Updates the expanded state and invokes <see cref="ExpandedChanged"/> if a delegate
    /// is bound, otherwise requests a state update. Does nothing if the state is unchanged.
    /// </summary>
    private async Task RaiseExpandedChanged(bool expanded)
    {
        if (IsExpanded == expanded) return;

        IsExpanded = expanded;

        if (ExpandedChanged.HasDelegate)
            await ExpandedChanged.InvokeAsync(expanded);
        else
            await InvokeAsync(StateHasChanged);

    }

    /// <summary>
    /// Builds the CSS class string for the trigger button, appending the code-focused
    /// modifier when applicable.
    /// </summary>
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

    /// <summary>
    /// Cleans up the component by removing this item from the parent <see cref="Accordion"/>.
    /// </summary>
    public void Dispose()
    
        => ParentControl?.RemoveAccordionItem(this);    
    
}
