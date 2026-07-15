using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.ActionsPopover.Tests.Unit.Common.Models;

public class LinkActionData_Tests
{

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("My Link")]
    [InlineData("  My Link   ")]
    public void Should_be_able_to_create_with_link_text_set_to_an_empty_string_if_null_or_whitespace(string? linkText)
    {
        var actionData = new LinkActionData<string>(linkText!, null);

        if (String.IsNullOrWhiteSpace(linkText))
        {
            actionData.LinkText.Should().Be(String.Empty);
            return;
        }

        actionData.LinkText.Should().Be(linkText.Trim());
    }

    [Fact]
    public void Should_be_able_to_create_with_the_specified_payload_type()
    {
        var actionData = new LinkActionData<int>("LinkText", 42);

        using (new AssertionScope())
        {
            actionData.Should().BeOfType<LinkActionData<int>>();

            actionData.GetValueOr(-42).Should().Be(42);
        }
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("my path")]
    [InlineData(" my path  ")]
    public void Should_be_able_to_create_with_a_path_that_defaults_to_a_forward_slash_if_empty_string_null_or_whitespace(string? path)
    {
        var actionData = new LinkActionData<string>("My Link",null,path: path!);

        if (String.IsNullOrWhiteSpace(path))
        {
            actionData.Path.Should().Be("/");
            return;
        }

        actionData.Path.Should().Be(path.Trim());
    }

    [Theory]
    [InlineData(PopoverLinkTargetType.Self)]
    [InlineData(PopoverLinkTargetType.Blank)]
    [InlineData(PopoverLinkTargetType.Top)]
    [InlineData(PopoverLinkTargetType.Parent)]
    public void Should_be_able_to_create_with_a_target_type(PopoverLinkTargetType targetType)
    {
        var actionData = new LinkActionData<string>("My Link", null,targetType);

        actionData.TargetType.Should().Be(targetType);
    }

    [Fact]
    public void Target_type_should_default_te_self_if_not_specified()
    {
        var actionData = new LinkActionData<string>("My Link", null);

        actionData.TargetType.Should().Be(PopoverLinkTargetType.Self);
    }


    [Theory]
    [InlineData(null)]
    [InlineData(42)]
    public void Get_value_or_should_give_the_value_if_not_null_or_use_the_provide_value_if_null(int? intValue)
    {
        var actionData = new LinkActionData<int?>("My Link", intValue);

        if (intValue.HasValue)
        {
            actionData.GetValueOr(-42).Should().Be(intValue);
            return;
        }

        actionData.GetValueOr(-42).Should().Be(-42);
    }
}
