using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class Kbd
{
    public const string Base = "br-kbd";

    public static string Scheme(KbdScheme scheme)

        => scheme switch
        {
            KbdScheme.Neutral   => $"{Base}--neutral-scheme",
            KbdScheme.Primary   => $"{Base}--primary-scheme",
            KbdScheme.Secondary => $"{Base}--secondary-scheme",
            KbdScheme.Accent    => $"{Base}--accent-scheme",
            _                   => $"{Base}--inverted-scheme",
        };

    public static string FontSize(KbdFontSize fontSize)
    {
        return fontSize switch
        {
            KbdFontSize.Label   => $"{Base}--font-size-label",
            KbdFontSize.Regular => $"{Base}--font-size-one",
            KbdFontSize.Two     => $"{Base}--font-size-two",
            KbdFontSize.Three   => $"{Base}--font-size-three",
            _                   => $"{Base}--font-size-one",
        };
    }
}
