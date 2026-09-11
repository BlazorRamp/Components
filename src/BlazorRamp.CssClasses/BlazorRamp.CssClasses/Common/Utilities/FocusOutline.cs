using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-focus-outline</c> block: a reusable
/// focus indicator built from the <c>--br-unit-outline-*</c> and
/// <c>--br-unit-outline-offset-*</c> primitive scales. By default the outline is only
/// shown for keyboard/assistive-technology focus (via <c>:focus-visible</c>).
/// </summary>
public static class FocusOutline
{
    /// <summary>
    /// The base class (BEM) block, <c>br-focus-outline</c>. Always required.
    /// </summary>
    public const string Base = "br-focus-outline";

    /// <summary>
    /// Suppresses the focus outline entirely, for both mouse and keyboard focus.
    /// </summary>
    public const string Off = $"{Base}--off";

    /// <summary>
    /// Shows the outline on any focus (<c>:focus</c>), not just keyboard/assistive-technology
    /// focus. Use sparingly — this reintroduces the outline for mouse users too.
    /// </summary>
    public const string OnFocus = $"{Base}--on-focus";

    /// <summary>
    /// Gets the class that sets the focus outline's width.
    /// </summary>
    /// <param name="unitOutlineWidth">The outline width to apply. See <see cref="UnitOutlineWidth"/>.</param>
    /// <returns>The <c>br-focus-outline--width-*</c> modifier class for the given <paramref name="unitOutlineWidth"/>.</returns>
    public static string Width(UnitOutlineWidth unitOutlineWidth)

        => unitOutlineWidth switch
        {
            UnitOutlineWidth.None => $"{Base}--width-0",
            UnitOutlineWidth.One => $"{Base}--width-1",
            UnitOutlineWidth.Two => $"{Base}--width-2",
            UnitOutlineWidth.Three => $"{Base}--width-3",
            UnitOutlineWidth.Four => $"{Base}--width-4",
            UnitOutlineWidth.Five => $"{Base}--width-5",
            _ => $"{Base}--width-2",
        };

    /// <summary>
    /// Gets the class that sets the focus outline's offset from the element's edge.
    /// </summary>
    /// <param name="unitOutlineOffset">The outline offset to apply. See <see cref="UnitOutlineOffset"/>.</param>
    /// <returns>The <c>br-focus-outline--offset-*</c> modifier class for the given <paramref name="unitOutlineOffset"/>.</returns>
    public static string Offset(UnitOutlineOffset unitOutlineOffset)

        => unitOutlineOffset switch
        {
            UnitOutlineOffset.None => $"{Base}--offset-0",
            UnitOutlineOffset.One => $"{Base}--offset-1",
            UnitOutlineOffset.Two => $"{Base}--offset-2",
            UnitOutlineOffset.Three => $"{Base}--offset-3",
            UnitOutlineOffset.Four => $"{Base}--offset-4",
            UnitOutlineOffset.Five => $"{Base}--offset-5",
            _ => $"{Base}--offset-2",
        };
}
