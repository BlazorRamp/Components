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
            SectionText.NeutralDark   => $"{Base}--neutral-text-dark",
            SectionText.SecondaryDark => $"{Base}--secondary-text-dark",
            SectionText.AccentDark    => $"{Base}--accent-text-dark",
            SectionText.PrimaryDark   => $"{Base}--primary-text-dark",
            _ => $"{Base}--accent-text-dark"
        };


    /// <summary>
    /// Gets the class that sets the section's base font size.
    /// </summary>
    /// <param name="unitFontSize">The font size to apply. See <see cref="UnitFontSize"/>.</param>
    /// <returns>The <c>br-section</c> modifier class for the given <paramref name="unitFontSize"/>.</returns>
    public static string FontSize(UnitFontSize unitFontSize)

        => unitFontSize switch
        {
            UnitFontSize.Label   => $"{Base}--font-size-label",
            UnitFontSize.Regular => $"{Base}--font-size-one",
            UnitFontSize.Two     => $"{Base}--font-size-two",
            UnitFontSize.Three   => $"{Base}--font-size-three",
            UnitFontSize.Four    => $"{Base}--font-size-four",
            UnitFontSize.Five    => $"{Base}--font-size-five",
            UnitFontSize.Six     => $"{Base}--font-size-six",
            _                    => $"{Base}--font-size-one"
        };

    /// <summary>
    /// Gets the class that sets the section's border radius to a fixed <c>--br-unit-radius-*</c>
    /// value, bypassing the themeable content/container/dialog groupings.
    /// </summary>
    /// <param name="unitRadius">The fixed radius to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-section</c> modifier class for the given <paramref name="unitRadius"/>.</returns>
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
                 SectionHeadingText.PrimaryDark   => $"{Base}--primary-text-dark",
                 SectionHeadingText.SecondaryDark => $"{Base}--secondary-text-dark",
                 SectionHeadingText.AccentDark    => $"{Base}--accent-text-dark",
                 SectionHeadingText.NeutralDark   => $"{Base}--neutral-text-dark",
                 _ => $"{Base}--accent-text-dark"
             };


        /// <summary>
        /// Gets the class that sets the heading's font size.
        /// </summary>
        /// <param name="unitFontSize">The font size to apply. See <see cref="UnitFontSize"/>.</param>
        /// <returns>The <c>br-section__heading</c> modifier class for the given <paramref name="unitFontSize"/>.</returns>
        public static string FontSize(UnitFontSize unitFontSize)

            => unitFontSize switch
            {
                UnitFontSize.Label   => $"{Base}--font-size-label",
                UnitFontSize.Regular => $"{Base}--font-size-one",
                UnitFontSize.Two     => $"{Base}--font-size-two",
                UnitFontSize.Three   => $"{Base}--font-size-three",
                UnitFontSize.Four    => $"{Base}--font-size-four",
                UnitFontSize.Five    => $"{Base}--font-size-five",
                UnitFontSize.Six     => $"{Base}--font-size-six",
                _ => $"{Base}--font-size-two",
            };
    }

}
