namespace BlazorRamp.NavGroup.Common.Constants;

internal class GlobalValues
{
    public const string Nav_Group_Class               = "br-nav-group";
    public const string Nav_Group_Trigger_Class       = $"{Nav_Group_Class}__trigger";
    public const string Nav_Group_Trigger_Icon_Class  = $"{Nav_Group_Class}__trigger-icon";
    public const string Nav_Group_Icon_Class          = $"{Nav_Group_Class}__icon";
    public const string Nav_Group_Title_Class         = $"{Nav_Group_Class}__title";
    public const string Nav_Group_Content_Class       = $"{Nav_Group_Class}__content";
    public const string Nav_Group_Link_Class          = $"{Nav_Group_Class}__link";
    public const string Nav_Group_Link_Text_Class     = $"{Nav_Group_Class}__link-text";
    public const string Nav_Group_Separator_Class     = $"{Nav_Group_Class}__separator";

    public const string Nav_Group_Svg_Css_Variable_Name   = "--_br-svg-nav-group-icon";
    public const string Nav_Group_Depth_CSS_Variable_Name = "--_br-svg-nav-group-depth";


    public const string LinkText_Missing_Message = "Link text is required and cannot be null, empty or whitespace";
    public const string LinkHref_Missing_Message = "Href is required and cannot be null, empty or whitespace";
    public const string SectionTitle_Missing_Message = "Title is required and cannot be null, empty or whitespace";


    /// <summary>
    /// Validates that a SvgIcon parameter and returns a CSS inline style string
    /// setting the internal SVG custom property, or <see langword="null"/> if the value is
    /// absent or does not begin with <c>--</c>.
    /// </summary>
    internal static string? CheckSetSvgVariable(string? svgIcon)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{GlobalValues.Nav_Group_Svg_Css_Variable_Name}:{iconVariable};";
    }

    /// <summary>
    /// Returns a CSS inline style string setting the depth custom property to the
    /// supplied <paramref name="depth"/> value, used to drive indentation in SCSS.
    /// </summary>
    /// <param name="depth">The zero-based nesting depth of the current navigation item.</param>
    internal static string? GetDepthStyleVariable(int depth)

        => $"{GlobalValues.Nav_Group_Depth_CSS_Variable_Name}:{depth};";
}
