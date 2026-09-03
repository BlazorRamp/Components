using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class Button
{
    public const string Base = "br-button";
    public const string FullWidth = $"{Base}--full-width";


    public static string Size(ButtonSize buttonSize)

        => buttonSize switch
        {
             ButtonSize.Small      => $"{Base}--small",
             ButtonSize.Regular    => $"{Base}--regular",
             ButtonSize.Large      => $"{Base}--large",
             ButtonSize.ExtraLarge => $"{Base}--extra-large",
             _                     => $"{Base}--regular"
        };


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

}
