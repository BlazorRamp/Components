using BlazorRamp.GoTo.Common.Constants;
using BlazorRamp.GoTo.Common.Models;
using FluentAssertions;

namespace BlazorRamp.GoTo.Tests.Unit.Common.Models;

public class GoToShortcut_Tests
{
    [Fact]
    public void Should_be_able_to_construct_a_new_go_to_shortcut_model()
    {
        var shortcut = new GoToShortcut("g", Alt: true, Ctrl: true, Shift: true, Meta: true);

        shortcut.Should().Match<GoToShortcut>(m => m.Key == "g" && m.Alt && m.Ctrl && m.Shift && m.Meta);
    }

    [Fact]
    public void Should_default_all_the_modifiers_to_false()
    {
        var shortcut = new GoToShortcut("g");

        shortcut.Should().Match<GoToShortcut>(m => !m.Alt && !m.Ctrl && !m.Shift && !m.Meta);
    }

    [Theory]
    [InlineData("g", true, false, false, false, true)]
    [InlineData("G", true, false, false, false, true)]    // case doesn't matter
    [InlineData("7", false, true, false, false, true)]    // digits are allowed
    [InlineData("y", false, true, true, false, true)]
    [InlineData("g", false, false, false, true, true)]
    [InlineData("g", false, false, false, false, false)]   // no modifier
    [InlineData("g", false, false, true, false, false)]   // Shift alone isn't enough
    [InlineData("gg", true, false, false, false, false)]   // more than one character
    [InlineData("", true, false, false, false, false)]   // no key
    [InlineData("!", true, false, false, false, false)]   // not a letter or digit
    [InlineData(" ", true, false, false, false, false)]
    [InlineData("é", true, false, false, false, false)]   // letters outside A-Z
    [InlineData(null, true, false, false, false, false)]
    public void Should_only_be_valid_with_one_letter_or_digit_and_a_modifier_other_than_shift(
        string? key, bool alt, bool ctrl, bool shift, bool meta, bool expected)
    {
        new GoToShortcut(key!, alt, ctrl, shift, meta).IsValid().Should().Be(expected);
    }

    [Theory]
    [InlineData("g", true, false, false, false, "Alt+G")]
    [InlineData("y", false, true, true, false, "Control+Shift+Y")]
    [InlineData("7", false, false, false, true, "Meta+7")]
    [InlineData("k", true, true, true, true, "Control+Alt+Shift+Meta+K")]
    public void Should_format_the_shortcut_for_aria_keyshortcuts(
        string key, bool alt, bool ctrl, bool shift, bool meta, string expected)
    {
        new GoToShortcut(key, alt, ctrl, shift, meta).ToAriaKeyShortcuts().Should().Be(expected);
    }

    [Fact]
    public void Should_compare_by_value()
    {
        var shortcut = new GoToShortcut("g", Alt: true);

        using (new FluentAssertions.Execution.AssertionScope())
        {
            shortcut.Should().Be(new GoToShortcut("g", Alt: true));
            shortcut.Should().Be(shortcut with { });
            shortcut.Should().NotBe(new GoToShortcut("g", Ctrl: true));
            shortcut.Should().NotBe(new GoToShortcut("h", Alt: true));
        }
    }

    [Fact]
    public void Should_have_a_valid_default_shortcut_of_alt_and_g()
    {
        var shortcut = GlobalValues.GoTo_Default_Shortcut_Model;

        using (new FluentAssertions.Execution.AssertionScope())
        {
            shortcut.IsValid().Should().BeTrue();
            shortcut.ToAriaKeyShortcuts().Should().Be("Control+Shift+Y");
        }
    }
}