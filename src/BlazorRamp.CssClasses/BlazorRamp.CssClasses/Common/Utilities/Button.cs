using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-button</c> block: a theme-reactive
/// button element that shares its underlying styling with every Blazor Ramp component.
/// </summary>
public static class Button
{
    /// <summary>
    /// The base class, <c>br-button</c>. Always required.
    /// </summary>
    public const string Base = "br-button";

    /// <summary>
    /// Makes the button take the full width of its container.
    /// </summary>
    public const string FullWidth = $"{Base}--full-width";

    /// <summary>
    /// Gets the class that sets the button's size.
    /// </summary>
    /// <param name="buttonSize">The size to apply. See <see cref="ButtonSize"/>.</param>
    /// <returns>The <c>br-button</c> modifier class for the given <paramref name="buttonSize"/>.</returns>
    public static string Size(ButtonSize buttonSize)

        => buttonSize switch
        {
             ButtonSize.Small      => $"{Base}--small",
             ButtonSize.Regular    => $"{Base}--regular",
             ButtonSize.Large      => $"{Base}--large",
             ButtonSize.ExtraLarge => $"{Base}--extra-large",
             _                     => $"{Base}--regular"
        };

    /// <summary>
    /// Gets the class that sets the button's solid colour scheme: its background,
    /// border, and a contrasting text colour.
    /// </summary>
    /// <param name="solidScheme">The solid colour scheme to apply. See <see cref="ButtonSolidScheme"/>.</param>
    /// <returns>The <c>br-button</c> modifier class for the given <paramref name="solidScheme"/>.</returns>
    public static string Scheme(ButtonSolidScheme solidScheme)

        => solidScheme switch 
        { 
            ButtonSolidScheme.Accent           => $"{Base}--solid-accent",
            ButtonSolidScheme.AccentLighter    => $"{Base}--solid-accent-lighter",
            ButtonSolidScheme.AccentDarker     => $"{Base}--solid-accent-darker",
            ButtonSolidScheme.Neutral          => $"{Base}--solid-neutral",
            ButtonSolidScheme.NeutralLighter   => $"{Base}--solid-neutral-lighter",
            ButtonSolidScheme.NeutralDarker    => $"{Base}--solid-neutral-darker",
            ButtonSolidScheme.Secondary        => $"{Base}--solid-secondary",
            ButtonSolidScheme.SecondaryLighter => $"{Base}--solid-secondary-lighter",
            ButtonSolidScheme.SecondaryDarker  => $"{Base}--solid-secondary-darker",
            ButtonSolidScheme.Primary          => $"{Base}--solid-primary",
            ButtonSolidScheme.PrimaryLighter   => $"{Base}--solid-primary-lighter",
            ButtonSolidScheme.PrimaryDarker    => $"{Base}--solid-primary-darker",
            ButtonSolidScheme.Danger           => $"{Base}--solid-danger",
            ButtonSolidScheme.DangerLighter    => $"{Base}--solid-danger-lighter",
            ButtonSolidScheme.DangerDarker     => $"{Base}--solid-danger-darker",
            ButtonSolidScheme.Warning          => $"{Base}--solid-warning",
            ButtonSolidScheme.WarningLighter   => $"{Base}--solid-warning-lighter",
            ButtonSolidScheme.WarningDarker    => $"{Base}--solid-warning-darker",
            ButtonSolidScheme.Success          => $"{Base}--solid-success",
            ButtonSolidScheme.SuccessLighter   => $"{Base}--solid-success-lighter",
            ButtonSolidScheme.SuccessDarker    => $"{Base}--solid-success-darker",
            ButtonSolidScheme.Info             => $"{Base}--solid-info",
            ButtonSolidScheme.InfoLighter      => $"{Base}--solid-info-lighter",
            ButtonSolidScheme.InfoDarker       => $"{Base}--solid-info-darker",
            ButtonSolidScheme.Inverted         => $"{Base}--solid-inverted",
            _                                  =>  $"{Base}--solid-primary",
        };


    /// <summary>
    /// Gets the class that sets the elements border radius to a fixed <c>--br-unit-radius-*</c>
    /// value, bypassing the themeable button border grouping that is used by default..
    /// </summary>
    /// <param name="unitRadius">The fixed radius to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-button</c> modifier class for the given <paramref name="unitRadius"/>.</returns>
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