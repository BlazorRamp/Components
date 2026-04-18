using BlazorRamp.NavGroup.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorRamp.NavGroup.Components;

/// <summary>
/// Renders an individual navigation link as an <c>&lt;li&gt;</c>
/// containing an <c>&lt;a&gt;</c> element. Automatically detects whether its
/// <see cref="Href"/> matches the current page and applies <c>aria-current="page"</c>
/// accordingly. Must be used inside a <see cref="NavSection"/> or <see cref="NavGroup"/>.
/// </summary>
public partial class NavGroupLink :IDisposable
{
    /// <summary>
    /// Gets or sets the cascading navigation depth, used to compute the indentation
    /// CSS custom property on the rendered list item. Supplied automatically by a
    /// parent <see cref="NavSection"/> via <c>CascadingValue Name="NavDepth"</c>.
    /// </summary>
    [CascadingParameter(Name = "NavDepth")] private int Depth { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="NavSection"/> component. Populated
    /// automatically via cascading value and used to register this link with its
    /// containing section.
    /// </summary>
    [CascadingParameter] public NavSection? ParentControl { get; set; } = default;

    /// <summary>
    /// Gets or sets the visible label rendered inside the link. This parameter is
    /// required and cannot be null, empty, or whitespace.
    /// </summary>
    [Parameter] public required string LinkText     { get; set; }

    /// <summary>
    /// Gets or sets the URL the link navigates to. This parameter is required and
    /// cannot be null, empty, or whitespace.
    /// </summary>
    [Parameter] public required string Href         { get; set; }

    /// <summary>
    /// Gets or sets an optional prefix rendered as visually hidden text before the
    /// main link label, providing additional context for screen reader users.
    /// Defaults to <see cref="String.Empty"/>.
    /// </summary>
    [Parameter] public string VisuallyHiddenPrefix  { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Gets or sets additional attributes applied to the anchor element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private NavigationManager? NavigationManager { get; set; }


    internal string CheckedHref     { get; set; } = String.Empty;
    internal string CheckedLinkText { get; set; } = String.Empty;


    private string? _linkSvgIcon = null;
    private bool   IsCurrentPage { get; set; } = false;

    /// <summary>
    /// Validates parameters on each render cycle, throwing if <see cref="LinkText"/>
    /// or <see cref="Href"/> is null, empty, or whitespace, and resolving the SVG
    /// icon style variable.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="LinkText"/> or <see cref="Href"/> is null, empty,
    /// or whitespace.
    /// </exception>
    protected override void OnParametersSet()
    {
        _linkSvgIcon = GlobalValues.CheckSetSvgVariable(SvgIcon);

        CheckedLinkText = String.IsNullOrWhiteSpace(LinkText) ? throw new ArgumentNullException(nameof(NavGroupLink.LinkText),GlobalValues.LinkText_Missing_Message) : LinkText.Trim();
        CheckedHref     = String.IsNullOrWhiteSpace(Href)     ? throw new ArgumentNullException(nameof(NavGroupLink.Href), GlobalValues.LinkHref_Missing_Message) : Href.Trim();
    }

    /// <summary>
    /// Registers this link with the parent <see cref="NavSection"/> and subscribes to
    /// <see cref="NavigationManager.LocationChanged"/> to track the current page.
    /// </summary>
    protected override void OnInitialized()
    {
        ParentControl?.AddGroupLink(this);

        if (NavigationManager is not null)
        {
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
            CheckSetCurrentPage(new Uri(NavigationManager.Uri));
        }
    }

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
   
        => CheckSetCurrentPage(new Uri(e.Location));


    private void CheckSetCurrentPage(Uri location)
    {
        var target = NavigationManager?.ToAbsoluteUri(Href);

        IsCurrentPage = (location.AbsolutePath.Equals(target?.AbsolutePath, StringComparison.OrdinalIgnoreCase)) ? true : false;
        StateHasChanged();
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
