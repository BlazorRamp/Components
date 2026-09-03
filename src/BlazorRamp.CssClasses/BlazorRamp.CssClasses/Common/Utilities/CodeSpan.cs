using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-code-span</c> block: an inline element
/// used to present a short fragment of code within a line of text.
/// </summary>
public static class CodeSpan
{
    /// <summary>
    /// The base class, <c>br-code-span</c>. Always required.
    /// </summary>
    public const string Base = "br-code-span";

    /// <summary>
    /// Gets the class that sets the code span's font size.
    /// </summary>
    /// <param name="codeSpanFontSize">The font size to apply. See <see cref="CodeSpanFontSize"/>.</param>
    /// <returns>The <c>br-code-span</c> modifier class for the given <paramref name="codeSpanFontSize"/>.</returns>
    public static string FontSize(CodeSpanFontSize codeSpanFontSize)

        => codeSpanFontSize switch
        {
            CodeSpanFontSize.Label   => $"{Base}--font-size-label",
            CodeSpanFontSize.Regular => $"{Base}--font-size-one",
            CodeSpanFontSize.Two     => $"{Base}--font-size-two",
            CodeSpanFontSize.Three   => $"{Base}--font-size-three",
            CodeSpanFontSize.Four    => $"{Base}--font-size-four",
            CodeSpanFontSize.Five    => $"{Base}--font-size-five",
            CodeSpanFontSize.Six     => $"{Base}--font-size-six",
            _ => $"{Base}--font-size-one"
        };

    /// <summary>
    /// Gets the class that sets the code span's colour scheme.
    /// </summary>
    /// <param name="scheme">The colour scheme to apply. See <see cref="CodeSpanScheme"/>.</param>
    /// <returns>The <c>br-code-span</c> modifier class for the given <paramref name="scheme"/>.</returns>
    public static string Scheme(CodeSpanScheme scheme)

    => scheme switch
    {
        CodeSpanScheme.Neutral => $"{Base}--neutral-scheme",
        CodeSpanScheme.Primary => $"{Base}--primary-scheme",
        CodeSpanScheme.Secondary => $"{Base}--secondary-scheme",
        CodeSpanScheme.Accent => $"{Base}--accent-scheme",
        _ => $"{Base}--inverted-scheme",
    };
}
