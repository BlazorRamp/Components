using Microsoft.AspNetCore.Components;

namespace BlazorRamp.NavGroup.Components;

/// <summary>
/// Renders a navigation group as a <c>&lt;ul&gt;</c>
/// element, acting as the root container for <see cref="NavSection"/> and
/// <see cref="NavGroupLink"/> child components.
/// </summary>
public partial class NavGroup
{
    /// <summary>
    /// Gets or sets the child content of the component, expected to contain one or more
    /// <see cref="NavSection"/>, <see cref="NavGroupLink"/>, or <see cref="NavSeparator"/>
    /// components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent  { get; set; }

    /// <summary>
    /// Gets or sets the <c>id</c> of an external element that labels this navigation
    /// group. When set, an <c>aria-labelledby</c> attribute referencing that element is
    /// applied to the root <c>&lt;ul&gt;</c>, providing an accessible name for screen
    /// readers. Leading and trailing whitespace is trimmed before use.
    /// </summary>
    [Parameter] public string?         AriaLabelledBy { get; set; }


    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

}


