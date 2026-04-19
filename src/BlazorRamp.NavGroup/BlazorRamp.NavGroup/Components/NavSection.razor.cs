using BlazorRamp.NavGroup.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorRamp.NavGroup.Components;

/// <summary>
/// Renders a collapsible navigation section as an <c>&lt;li&gt;</c>
/// containing a trigger <c>&lt;button&gt;</c> and a nested <c>&lt;ul&gt;</c> of child items.
/// Sections can be nested to create a multi-level navigation hierarchy and automatically
/// expand when a descendant link matches the current page.
/// Must be used inside a <see cref="NavGroup"/> or another <see cref="NavSection"/>.
/// </summary>
public partial class NavSection : IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="NavSection"/> component. Populated
    /// automatically via cascading value and used to propagate expand state up the
    /// component tree when a descendant link matches the current page.
    /// </summary>
    [CascadingParameter] public NavSection? ParentControl { get; set; } = default;

    /// <summary>
    /// Gets or sets the cascading navigation depth, used to compute the indentation
    /// CSS custom property on the rendered list item. Supplied automatically by a
    /// parent <see cref="NavSection"/> via <c>CascadingValue Name="NavDepth"</c>.
    /// </summary>
    [CascadingParameter(Name = "NavDepth")] private int Depth { get; set; } = 0;

    /// <summary>
    /// Gets or sets the child content rendered inside the section's collapsible
    /// <c>&lt;ul&gt;</c> element, expected to contain one or more
    /// <see cref="NavGroupLink"/>, <see cref="NavSection"/>, or
    /// <see cref="NavSeparator"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; } = default;

    /// <summary>
    /// Gets or sets the visible label rendered in the section trigger button.
    /// </summary>
    [Parameter] public string?         Title        { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the section is expanded on initial
    /// render. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter] public bool            Expanded     { get; set; }

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;
   
    [Inject] NavigationManager? NavigationManager { get; set; }

    internal string CheckedTitle { get; set; } = String.Empty;
    private int ChildDepth => Depth + 1;

    private List<NavSection>   _childSections = [];
    private List<NavGroupLink> _groupLinks  = [];

    private string  _contentID      = Guid.NewGuid().ToString();
    private string  _buttonID       = Guid.NewGuid().ToString();
    private bool    _expanded       = false;
    private string? _sectionSvgIcon = null;


    /// <summary>
    /// Validates parameters on each render cycle, throwing if <see cref="Title"/>
    /// is null, empty, or whitespace, and resolving the SVG icon style variable.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="Title"/> is null, empty, or whitespace.
    /// </exception>
    protected override void OnParametersSet()
    {
        _sectionSvgIcon = GlobalValues.CheckSetSvgVariable(SvgIcon);
        CheckedTitle    = String.IsNullOrWhiteSpace(Title) ? throw new ArgumentNullException(nameof(NavSection.Title), GlobalValues.SectionTitle_Missing_Message) : Title.Trim();

    }

    /// <summary>
    /// Registers this section with its parent <see cref="NavSection"/> and subscribes
    /// to <see cref="NavigationManager.LocationChanged"/> to detect current-page changes
    /// in descendant links.
    /// </summary>
    protected override void OnInitialized()
    {
        ParentControl?.AddChildSection(this);
        _expanded = Expanded;
        if (NavigationManager is not null) NavigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    
        => CheckSetCurrentPage(new Uri(e.Location), _groupLinks, NavigationManager!);
    

    private void CheckSetCurrentPage(Uri location, List<NavGroupLink> groupLinks, NavigationManager navigationManager)
    {
        foreach (var link in _groupLinks)
        {
            var target = NavigationManager?.ToAbsoluteUri(link.Href);

            if (location.AbsolutePath.Equals(target?.AbsolutePath, StringComparison.OrdinalIgnoreCase))
            {
                SetExpandedState(true);
                ParentControl?.ExpandParent();
                break;
            }
        }
    }

    /// <summary>
    /// Registers a child <see cref="NavSection"/> with this section. Called
    /// automatically by each direct child section during initialisation.
    /// </summary>
    internal void AddChildSection(NavSection childSection)
    {
        if (false == _childSections.Contains(childSection)) _childSections.Add(childSection);
    }

    /// <summary>
    /// Registers a <see cref="NavGroupLink"/> with this section. Called automatically
    /// by each direct child link during initialisation.
    /// </summary>
    internal void AddGroupLink(NavGroupLink groupLink)
    {
        if (false == _groupLinks.Contains(groupLink)) _groupLinks.Add(groupLink);
    }

    /// <summary>
    /// Sets the expanded state of this section and requests a state update.
    /// </summary>
    /// <param name="expanded">
    /// <see langword="true"/> to expand the section; <see langword="false"/> to collapse it.
    /// </param>
    public void SetExpandedState(bool expanded)
    {
        _expanded = expanded;
        StateHasChanged();
    }

    /// <summary>
    /// On first render, checks whether any registered descendant link matches the
    /// current URI and expands the section if so.
    /// </summary>
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && NavigationManager is not null)
        {
            CheckSetCurrentPage(new Uri(NavigationManager.Uri), _groupLinks, NavigationManager!);
        }
    }

    /// <summary>
    /// Expands this section and recursively expands each ancestor
    /// <see cref="NavSection"/>, ensuring the full path to the current page is visible.
    /// </summary>
    internal void ExpandParent()
    {
        SetExpandedState(true);

        ParentControl?.ExpandParent();

    }
    /// <summary>
    /// Unsubscribes from <see cref="NavigationManager.LocationChanged"/> to prevent
    /// memory leaks when the component is removed from the render tree.
    /// </summary>
    public void Dispose()
    {
        try
        {
            if (NavigationManager is not null) NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }
        catch { }

    }
}
