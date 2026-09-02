using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-section</c> block: a general-purpose,
/// theme-reactive content section used for page content, cards, and other bordered
/// or coloured content areas.
/// </summary>
public static class Section
{
    /// <summary>
    /// The base class (BEM) block, <c>br-section</c>. Always required.
    /// </summary>
    public const string Base     = "br-section";

    /// <summary>
    /// Adds a visible border using the shared divider colour.
    /// </summary>
    public const string Bordered = $"{Base}--bordered";

    /// <summary>
    /// Removes the default section's base padding.
    /// </summary>
    public const string NoPadding = $"{Base}--no-padding";

    /// <summary>
    /// Switches the section's font family to the font used for code font.
    /// </summary>
    public const string CodeFont  = $"{Base}--code-font";

    /// <summary>
    /// Removes the default block-direction (top/bottom) margin from the section.
    /// </summary>
    public const string NoMarginBlock = $"{Base}--no-margin-block";

    /// <summary>
    /// Lays out the section's content as a centred flex column.
    /// </summary>
    public const string ColumnCentred = $"{Base}--column-centred";

    /// <summary>
    /// Gets the class that sets the section's border radius.
    /// </summary>
    /// <param name="sectionRadius">The border radius grouping to apply. See <see cref="SectionRadius"/>.</param>
    /// <returns>The <c>br-section</c> modifier class for the given <paramref name="sectionRadius"/>.</returns>
    public static string Radius(SectionRadius sectionRadius)

        => sectionRadius switch
        {
            SectionRadius.Container => $"{Base}--container",
            SectionRadius.Dialog => $"{Base}--dialog",
            _ => $"{Base}--content",
        };

    /// <summary>
    /// Gets the class that sets the section's background colour.
    /// </summary>
    /// <param name="sectionBackground">The background colour to apply. See <see cref="SectionBackground"/>.</param>
    /// <returns>The <c>br-section</c> modifier class for the given <paramref name="sectionBackground"/>.</returns>
    public static string Background(SectionBackground sectionBackground)

        => sectionBackground switch
        {
            SectionBackground.PrimaryLighter    => $"{Base}--primary-lighter",
            SectionBackground.NeutralLighter    => $"{Base}--neutral-lighter",
            SectionBackground.SecondaryLighter  => $"{Base}--secondary-lighter",
            SectionBackground.AccentLighter     => $"{Base}--accent-lighter",
            _ => $"{Base}--neutral-lighter",

        };

    /// <summary>
    /// Gets the class that sets the section's text colour.
    /// </summary>
    /// <param name="sectionText">The text colour to apply. See <see cref="SectionText"/>.</param>
    /// <returns>The <c>br-section</c> modifier class for the given <paramref name="sectionText"/>.</returns>
    public static string Text(SectionText sectionText)

        => sectionText switch
        {
            SectionText.Neutral   => $"{Base}--neutral-text",
            SectionText.Secondary => $"{Base}--secondary-text",
            SectionText.Accent    => $"{Base}--accent-text",
            SectionText.Primary   => $"{Base}--primary-text",
            _ => $"{Base}--accent-text"
        };


    /// <summary>
    /// Gets the class that sets the section's base font size.
    /// </summary>
    /// <param name="sectionFontSize">The font size to apply. See <see cref="SectionFontSize"/>.</param>
    /// <returns>The <c>br-section</c> modifier class for the given <paramref name="sectionFontSize"/>.</returns>
    public static string FontSize(SectionFontSize sectionFontSize)

        => sectionFontSize switch
        {
            SectionFontSize.Label => $"{Base}--font-size-label",
            SectionFontSize.Regular => $"{Base}--font-size-one",
            SectionFontSize.Two => $"{Base}--font-size-two",
            SectionFontSize.Three => $"{Base}--font-size-three",
            _ => $"{Base}--font-size-one"
        };

    /// <summary>
    /// Provides the BEM CSS classes for the <c>br-section__heading</c> element,
    /// the heading sub-part of a <c>br-section</c>.
    /// </summary>
    public static class Heading
    {
        /// <summary>
        /// The base class, <c>br-section__heading</c>. Always required.
        /// </summary>
        public const string Base = $"{Section.Base}__heading";

        /// <summary>
        /// Centre-aligns the heading text below the small breakpoint.
        /// </summary>
        public const string MobileCentred = $"{Base}--mobile-centred";

        /// <summary>
        /// Gets the class that sets the heading's text colour.
        /// </summary>
        /// <param name="sectionHeadingText">The text colour to apply. See <see cref="SectionHeadingText"/>.</param>
        /// <returns>The <c>br-section__heading</c> modifier class for the given <paramref name="sectionHeadingText"/>.</returns>
        public static string Text(SectionHeadingText sectionHeadingText)

             => sectionHeadingText switch
             {
                 SectionHeadingText.Primary   => $"{Base}--primary-text",
                 SectionHeadingText.Secondary => $"{Base}--secondary-text",
                 SectionHeadingText.Accent    => $"{Base}--accent-text",
                 SectionHeadingText.Neutral   => $"{Base}--neutral-text",
                 _ => $"{Base}--accent-text"
             };


        /// <summary>
        /// Gets the class that sets the heading's font size.
        /// </summary>
        /// <param name="sectionHeadingFontSize">The font size to apply. See <see cref="SectionHeadingFontSize"/>.</param>
        /// <returns>The <c>br-section__heading</c> modifier class for the given <paramref name="sectionHeadingFontSize"/>.</returns>
        public static string FontSize(SectionHeadingFontSize sectionHeadingFontSize)

            => sectionHeadingFontSize switch
            {
                SectionHeadingFontSize.Regular => $"{Base}--font-size-one",
                SectionHeadingFontSize.Two     => $"{Base}--font-size-two",
                SectionHeadingFontSize.Three   => $"{Base}--font-size-three",
                SectionHeadingFontSize.Four    => $"{Base}--font-size-four",
                SectionHeadingFontSize.Five    => $"{Base}--font-size-five",
                SectionHeadingFontSize.Six     => $"{Base}--font-size-six",
                _ => $"{Base}--font-size-two",
            };
    }

}
