namespace BlazorRamp.GoTo.Common.Models;

/// <summary>
/// Describes one marker found on the page.
/// </summary>
/// <param name="MarkerID">The id of the marker element, which receives focus.</param>
/// <param name="PopoverID">The id of the popover that shows the marker's label.</param>
/// <param name="GoToText">The label text.</param>
/// <param name="GoToNumber">The two-digit number shown beside the label, such as <c>01</c>.</param>
public record GoToData(string MarkerID, string PopoverID, string GoToText, string GoToNumber);




/// <summary>
/// Describes the keyboard shortcut that opens the <see cref="Components.GoToViewer"/>. A shortcut is a single letter or digit
/// combined with one or more modifier keys, and it must include at least one of Alt, Ctrl or Meta.
/// </summary>
/// <param name="Key">
/// The letter or digit pressed together with the modifiers, for example <c>g</c>. It is matched without regard to case, and
/// it is also matched by the physical key position so that it still works when a modifier such as Option on macOS types a different character.
/// </param>
/// <param name="Alt">
/// <see langword="true"/> if the Alt key (Option on macOS) must be held; otherwise <see langword="false"/>. Defaults to <see langword="false"/>.
/// </param>
/// <param name="Ctrl">
/// <see langword="true"/> if the Control key must be held; otherwise <see langword="false"/>. Defaults to <see langword="false"/>.
/// </param>
/// <param name="Shift">
/// <see langword="true"/> if the Shift key must be held; otherwise <see langword="false"/>. Defaults to <see langword="false"/>.
/// </param>
/// <param name="Meta">
/// <see langword="true"/> if the Meta key (Windows key, or Command on macOS) must be held; otherwise <see langword="false"/>.
/// Defaults to <see langword="false"/>.
/// </param>
/// <remarks>
/// The modifiers must match exactly, so a shortcut defined as Alt+G isn't triggered by Alt+Shift+G.
/// </remarks>
public record GoToShortcut(string Key, bool Alt = false, bool Ctrl = false, bool Shift = false, bool Meta = false)
{
    /// <summary>
    /// Determines whether the shortcut can be used.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <see cref="Key"/> is a single letter or digit and at least one of <see cref="Alt"/>, <see cref="Ctrl"/>
    /// or <see cref="Meta"/> is set; otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// A modifier is required because a shortcut made up only of character keys, even with Shift, would fall under
    /// WCAG 2.1.4 (Character Key Shortcuts), which requires it to be turned off or remapped.
    /// </remarks>
    public bool IsValid() => Key is { Length: 1 } && char.IsAsciiLetterOrDigit(Key[0]) && (Alt || Ctrl || Meta);

    /// <summary>
    /// Formats the shortcut for the <c>aria-keyshortcuts</c> attribute, for example <c>Alt+G</c> or <c>Control+Shift+Y</c>.
    /// </summary>
    /// <returns>The modifier names followed by the upper case key, separated by plus signs.</returns>
    public string ToAriaKeyShortcuts()
    {
        var parts = new List<string>();

        if (Ctrl) parts.Add("Control");
        if (Alt) parts.Add("Alt");
        if (Shift) parts.Add("Shift");
        if (Meta) parts.Add("Meta");

        parts.Add(Key.ToUpperInvariant());

        return string.Join("+", parts);
    }
}