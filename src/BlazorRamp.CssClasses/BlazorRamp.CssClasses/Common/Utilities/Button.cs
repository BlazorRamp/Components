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
             _                               => $"{Base}--regular"
        };

}
