using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class Section
{
    public const string Base      = "br-section";
    public const string Bordered  = $"{Base}--bordered";
    public const string NoPadding = $"{Base}--no-padding";
    public const string CodeFont  = $"{Base}--code-font";



    public static string Radius(SectionRadius sectionRadius)

        => sectionRadius switch 
        { 
            SectionRadius.Container => $"{Base}--container",
            SectionRadius.Dialog    => $"{Base}--dialog",
            _                       => $"{Base}--content",
        };

    public static string Background(SectionBackground sectionBackground)

        => sectionBackground switch
        { 
            SectionBackground.Neutral20        => $"{Base}--neutral-20",
            SectionBackground.SecondaryLighter => $"{Base}--secondary-lighter",
            SectionBackground.AccentLighter    => $"{Base}--accent-lighter",
            _                                  => $"{Base}--neutral-10",

        };

    public static string Text(SectionText sectionText)

        => sectionText switch
        { 
            SectionText.SecondaryDark => $"{Base}--secondary-text",
            _                         => $"{Base}--accent-text" 
        };

    public static string FontSize(SectionFontSize sectionFontSize)

        => sectionFontSize switch
        {
            SectionFontSize.Label => $"{Base}--font-size-label",
            SectionFontSize.Two   => $"{Base}--font-size-two",
            _                     => $"{Base}--font-size-one"
        };



    public static class Heading
    {
        public const string Base = "br-section__heading";

        public const string MobileCentred = $"{Base}--mobile-centred";


        public static string Text(SectionHeadingText sectionHeadingText)

     => sectionHeadingText switch
     {
         SectionHeadingText.PrimaryDark   => $"{Base}--primary-text",
         SectionHeadingText.SecondaryDark => $"{Base}--secondary-text",
         _ => $"{Base}--accent-text"
     };

    }
}
