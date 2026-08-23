using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class CodeSpan
{
    public const string Base = "br-code-span";

    public static string FontSize(CodeSpanFontSize codeSpanFontSize)

        => codeSpanFontSize switch
        {
            CodeSpanFontSize.Label => $"{Base}--font-size-label",
            CodeSpanFontSize.Two => $"{Base}--font-size-two",
            CodeSpanFontSize.Three => $"{Base}-font-size-three",
            _ => $"{Base}--font-size-one"
        };
}
