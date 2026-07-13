using BlazorRamp.ActionsPopover.Common.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.ActionsPopover.Tests.Unit.Common.Models;

public class ButtonActionData_Tests
{

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("My Button")]
    [InlineData("  My Button   ")]
    public void Should_be_able_to_create_with_button_text_set_to_an_empty_string_if_null_or_whitespace(string? buttonText)
    {
        var actionData = new ButtonActionData<string>(buttonText!, null);

        if (String.IsNullOrWhiteSpace(buttonText))
        {
            actionData.ButtonText.Should().Be(String.Empty);
            return;
        }

        actionData.ButtonText.Should().Be(buttonText.Trim());
    }

    [Fact]
    public void Should_be_able_to_create_with_the_specified_payload_type()
    {
        var actionData = new ButtonActionData<int>("ButtonText", 42);

        using(new AssertionScope())
        {
            actionData.Should().BeOfType<ButtonActionData<int>>();

            actionData.GetValueOr(-42).Should().Be(42);
        }
    }
}
