using BlazorRamp.Tabs.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Tabs.Components;

/// <summary>
/// A Blazor component that renders an individual tab and its associated panel within a
/// <see cref="Tabs"/> component. Must be used as a direct child of <see cref="Tabs"/>.
/// </summary>
public partial class Tab : IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="Tabs"/> component. Populated automatically via cascading value.
    /// </summary>
    [CascadingParameter] public Tabs? ParentControl { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content rendered inside the tab panel.
    /// </summary>
    [Parameter] public RenderFragment? TabPanelContent { get; set; } = null;

    /// <summary>
    /// Gets or sets the visible label for the tab button. Also used as the accessible name.
    /// This parameter is required and cannot be null, empty, or whitespace.
    /// </summary>
    [Parameter, EditorRequired] public string TabTitle { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the tab panel content is retained in the DOM
    /// when the tab is not active. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool PersistContent { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon alongside the tab title. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Gets or sets a value indicating whether <c>tabindex="0"</c> is applied to the tab panel,
    /// enabling keyboard focus and arrow key scrolling if the panel has a constrained height.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool HasPanelTabIndex { get; set; } = true;



    internal string TabPanelID   { get; } = $"tab-panel-{Guid.NewGuid().ToString()}";
    internal string TabID        { get; } = $"tab-{Guid.NewGuid().ToString()}";
    internal string? SvgVariable { get; private set;}  = null;

    private string _cssClasses     = GlobalValues.Tabs_Tab_Panel_Class;
    private bool   _isActiveTab    = false;
    private bool   _performRender  = false;

    /// <summary>
    /// Validates parameters on each render cycle, throwing if <see cref="TabTitle"/> is null,
    /// empty or whitespace, and resolving the active tab state and CSS classes.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="TabTitle"/> is null, empty, or whitespace.
    /// </exception>
    protected override void OnParametersSet()
    {
       
        if (String.IsNullOrWhiteSpace(TabTitle)) throw new ArgumentNullException(nameof(TabTitle),GlobalValues.Error_Message_Tab_Title);

        SvgVariable = CheckSetSvgVariable(SvgIcon);

        _isActiveTab = ParentControl!.ActiveTab == this;

        if (true == _isActiveTab)
        {
            _performRender = true;
        }
        else if (false == PersistContent)
        {
            _performRender = false;
        }

        _cssClasses = _isActiveTab ? $"{GlobalValues.Tabs_Tab_Panel_Class} {GlobalValues.Tabs_Tab_Panel_Active_Modifier}"
                                   : GlobalValues.Tabs_Tab_Panel_Class ;
       
    }
    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if (ParentControl == null) throw new ArgumentNullException(GlobalValues.Error_Message_Needs_Tabs_Componenent);

        ParentControl.AddTab(this);
    }

    /// <summary>
    /// Validates the <see cref="SvgIcon"/> parameter and returns a CSS inline style string
    /// setting the internal SVG custom property, or <see langword="null"/> if the value is
    /// absent or does not begin with <c>--</c>.
    /// </summary>
    internal string? CheckSetSvgVariable(string? svgIcon)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon) 
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{GlobalValues.Tab_Svg_Css_Variable_Name}:{iconVariable};";
    }


    /// <summary>
    /// Cleans up the component by removing this tab from the parent <see cref="Tabs"/> component.
    /// </summary>
    public void Dispose()
        
        =>  ParentControl?.RemoveTab(this);

    
}
