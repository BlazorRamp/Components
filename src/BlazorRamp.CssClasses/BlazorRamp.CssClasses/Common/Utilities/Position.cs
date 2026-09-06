using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides utility classes that set an element's CSS <c>position</c>. Unlike the BEM
/// block classes elsewhere in this library, these are flat, single-purpose utility
/// classes intended to be combined freely with other classes on any element.
/// </summary>
public static class Position
{
    private const string _positionBase = "br-position";

    /// <summary>
    /// Gets the class that sets an element's CSS <c>position</c>.
    /// </summary>
    /// <param name="unitPosition">The position value to apply. See <see cref="UnitPosition"/>.</param>
    /// <returns>The <c>br-position-*</c> utility class for the given <paramref name="unitPosition"/>.</returns>
    public static string SetAs(UnitPosition unitPosition)

        => unitPosition switch
        {
            UnitPosition.Static   => $"{_positionBase}-static",
            UnitPosition.Relative => $"{_positionBase}-relative",
            UnitPosition.Absolute => $"{_positionBase}-absolute",
            UnitPosition.Fixed    => $"{_positionBase}-fixed",
            UnitPosition.Sticky   => $"{_positionBase}-sticky",
            _                     => $"{_positionBase}-relative",
        };
}