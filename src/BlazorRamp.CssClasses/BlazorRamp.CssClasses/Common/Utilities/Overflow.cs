using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides utility classes that set the CSS <c>overflow</c> behaviour of an element.
/// Unlike the BEM block classes elsewhere in this library, these are flat, single-purpose
/// utility classes intended to be combined freely with other classes on any element.
/// </summary>
/// <remarks>
/// If the element already has a component class with its own overflow modifiers
/// (for example <c>br-section</c>), prefer that class's own modifier instead - combining
/// both relies on CSS source order to determine which one wins.
/// </remarks>
public static class Overflow
{
    private const string _overflowBase = "br-overflow";

    /// <summary>
    /// Gets the class that sets an element's horizontal (<c>overflow-x</c>) scroll behaviour.
    /// </summary>
    /// <remarks>
    /// Setting this to anything other than <see cref="UnitScroll.Visible"/> may cause the
    /// vertical axis to compute as <c>auto</c> as well, per the CSS overflow specification,
    /// unless <see cref="Y"/> is also set explicitly.
    /// </remarks>
    /// <param name="unitScroll">The overflow value to apply. See <see cref="UnitScroll"/>.</param>
    /// <returns>The <c>br-overflow-x-*</c> utility class for the given <paramref name="unitScroll"/>.</returns>
    public static string X(UnitScroll unitScroll)

        => unitScroll switch
        {
            UnitScroll.Visible => $"{_overflowBase}-x-visible",
            UnitScroll.Hidden  => $"{_overflowBase}-x-hidden",
            UnitScroll.Scroll  => $"{_overflowBase}-x-scroll",
            UnitScroll.Clip    => $"{_overflowBase}-x-clip",
            UnitScroll.Auto    => $"{_overflowBase}-x-auto",
            _                  => $"{_overflowBase}-x-auto",
        };

    /// <summary>
    /// Gets the class that sets an element's vertical (<c>overflow-y</c>) scroll behaviour.
    /// </summary>
    /// <remarks>
    /// Setting this to anything other than <see cref="UnitScroll.Visible"/> may cause the
    /// horizontal axis to compute as <c>auto</c> as well, per the CSS overflow specification,
    /// unless <see cref="X"/> is also set explicitly.
    /// </remarks>
    /// <param name="unitScroll">The overflow value to apply. See <see cref="UnitScroll"/>.</param>
    /// <returns>The <c>br-overflow-y-*</c> utility class for the given <paramref name="unitScroll"/>.</returns>
    public static string Y(UnitScroll unitScroll)

        => unitScroll switch
        {
            UnitScroll.Visible => $"{_overflowBase}-y-visible",
            UnitScroll.Hidden  => $"{_overflowBase}-y-hidden",
            UnitScroll.Scroll  => $"{_overflowBase}-y-scroll",
            UnitScroll.Clip    => $"{_overflowBase}-y-clip",
            UnitScroll.Auto    => $"{_overflowBase}-y-auto",
            _                  => $"{_overflowBase}-y-auto",
        };

    /// <summary>
    /// Gets the class that sets an element's overflow behaviour on both axes at once,
    /// using the CSS <c>overflow</c> shorthand.
    /// </summary>
    /// <param name="unitScroll">The overflow value to apply. See <see cref="UnitScroll"/>.</param>
    /// <returns>The <c>br-overflow-xy-*</c> utility class for the given <paramref name="unitScroll"/>.</returns>
    public static string XY(UnitScroll unitScroll)

        => unitScroll switch
        {
            UnitScroll.Visible => $"{_overflowBase}-xy-visible",
            UnitScroll.Hidden  => $"{_overflowBase}-xy-hidden",
            UnitScroll.Scroll  => $"{_overflowBase}-xy-scroll",
            UnitScroll.Clip    => $"{_overflowBase}-xy-clip",
            UnitScroll.Auto    => $"{_overflowBase}-xy-auto",
            _                  => $"{_overflowBase}-xy-auto",
        };
}