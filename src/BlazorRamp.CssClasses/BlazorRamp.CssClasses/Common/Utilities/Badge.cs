using BlazorRamp.CssClasses.Common.Constants;
using static BlazorRamp.CssClasses.Common.Constants.BadgeEnums;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class Badge
{
    /// <summary>
    /// The base class, <c>br-button</c>. Always required.
    /// </summary>
    public const string Base = "br-badge";

    /// <summary>
    /// Gets the class that sets the badges's size.
    /// </summary>
    /// <param name="badgeSize">The size to apply. See <see cref="BadgeSize"/>.</param>
    /// <returns>The <c>br-button</c> modifier class for the given <paramref name="badgeSize"/>.</returns>
    public static string Size(BadgeSize badgeSize)

        => badgeSize switch
        {
            BadgeSize.Tiny       => $"{Base}--tiny",
            BadgeSize.Small      => $"{Base}--small",
            BadgeSize.Medium     => $"{Base}--medium",
            BadgeSize.Large      => $"{Base}--large",
            _ => $"{Base}--small"
        };

    /// <summary>
    /// Gets the class that sets the badge's solid colour scheme: its background,
    /// border, and a contrasting text colour.
    /// </summary>
    /// <param name="solidScheme">The solid colour scheme to apply. See <see cref="BadgeSolidScheme"/>.</param>
    /// <returns>The <c>br-badge</c> modifier class for the given <paramref name="solidScheme"/>.</returns>
    public static string Scheme(BadgeSolidScheme solidScheme)

        => solidScheme switch
        {
            BadgeSolidScheme.Accent           => $"{Base}--solid-accent",
            BadgeSolidScheme.AccentLighter    => $"{Base}--solid-accent-lighter",
            BadgeSolidScheme.AccentDarker     => $"{Base}--solid-accent-darker",
            BadgeSolidScheme.Neutral          => $"{Base}--solid-neutral",
            BadgeSolidScheme.NeutralLighter   => $"{Base}--solid-neutral-lighter",
            BadgeSolidScheme.NeutralDarker    => $"{Base}--solid-neutral-darker",
            BadgeSolidScheme.Secondary        => $"{Base}--solid-secondary",
            BadgeSolidScheme.SecondaryLighter => $"{Base}--solid-secondary-lighter",
            BadgeSolidScheme.SecondaryDarker  => $"{Base}--solid-secondary-darker",
            BadgeSolidScheme.Primary          => $"{Base}--solid-primary",
            BadgeSolidScheme.PrimaryLighter   => $"{Base}--solid-primary-lighter",
            BadgeSolidScheme.PrimaryDarker    => $"{Base}--solid-primary-darker",
            BadgeSolidScheme.Danger           => $"{Base}--solid-danger",
            BadgeSolidScheme.DangerLighter    => $"{Base}--solid-danger-lighter",
            BadgeSolidScheme.DangerDarker     => $"{Base}--solid-danger-darker",
            BadgeSolidScheme.Warning          => $"{Base}--solid-warning",
            BadgeSolidScheme.WarningLighter   => $"{Base}--solid-warning-lighter",
            BadgeSolidScheme.WarningDarker    => $"{Base}--solid-warning-darker",
            BadgeSolidScheme.Success          => $"{Base}--solid-success",
            BadgeSolidScheme.SuccessLighter   => $"{Base}--solid-success-lighter",
            BadgeSolidScheme.SuccessDarker    => $"{Base}--solid-success-darker",
            BadgeSolidScheme.Info             => $"{Base}--solid-info",
            BadgeSolidScheme.InfoLighter      => $"{Base}--solid-info-lighter",
            BadgeSolidScheme.InfoDarker       => $"{Base}--solid-info-darker",
            BadgeSolidScheme.Inverted         => $"{Base}--solid-inverted",
            _ => $"{Base}--solid-primary",
        };


    /// <summary>
    /// Gets the class that sets the elements border radius to a fixed <c>--br-unit-radius-*</c>
    /// </summary>
    /// <param name="unitRadius">The fixed radius to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-badge</c> modifier class for the given <paramref name="unitRadius"/>.</returns>
    public static string FixedRadius(UnitRadius unitRadius)

        => unitRadius switch
        {
            UnitRadius.None  => $"{Base}--radius-none",
            UnitRadius.One   => $"{Base}--radius-one",
            UnitRadius.Two   => $"{Base}--radius-two",
            UnitRadius.Three => $"{Base}--radius-three",
            UnitRadius.Four  => $"{Base}--radius-four",
            UnitRadius.Five  => $"{Base}--radius-five",
            UnitRadius.Six   => $"{Base}--radius-six",
            UnitRadius.Seven => $"{Base}--radius-seven",
            UnitRadius.Eight => $"{Base}--radius-eight",
            UnitRadius.Nine  => $"{Base}--radius-nine",
            UnitRadius.Full  => $"{Base}--radius-full",
            _ => $"{Base}--radius-none",
        };


}
