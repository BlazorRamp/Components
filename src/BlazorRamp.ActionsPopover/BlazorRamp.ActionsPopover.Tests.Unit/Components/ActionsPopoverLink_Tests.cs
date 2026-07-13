using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using BlazorRamp.ActionsPopover.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BlazorRamp.ActionsPopover.Tests.Unit.Components;

public class ActionPopoverLink_Tests
{
    public static IRenderedComponent<ActionPopoverLink<TData>> CreateActionPopoverLink<TData>(
        BunitContext context,
        Action<ComponentParameterCollectionBuilder<ActionPopoverLink<TData>>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

        moduleInterop.SetupVoid(GlobalValues.JS_Register_Prevent_Click_Action_Handler, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Unregister_Prevent_Click_Action_Handler, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Hide_Popover_Func, _ => true).SetVoidResult();

        var component = context.Render<ActionPopoverLink<TData>>(
            builder => parameters?.Invoke(builder));

        return component;
    }

    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_throw_when_link_text_is_null_empty_or_whitespace(string? linkText)
        {
            await using var context = new BunitContext();

            var act = () => CreateActionPopoverLink<string>(context, p => p.Add(x => x.LinkText, linkText!));

            act.Should().Throw<ArgumentNullException>()
               .WithMessage($"*{GlobalValues.Actions_Popover_Link_Text_Exception_Message}*");
        }

        [Fact]
        public async Task Should_render_the_link_text()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p.Add(x => x.LinkText, "View"));

            link.Find($".{GlobalValues.Actions_Popover_Action_Text_Class}").TextContent.Trim().Should().Be("View");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_default_the_href_to_root_when_path_is_null_empty_or_whitespace(string? path)
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.Path, path!));

            link.Find("a").GetAttribute("href").Should().Be("/");
        }

        [Fact]
        public async Task Should_trim_the_path_when_set()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.Path, "  /items/42  "));

            link.Find("a").GetAttribute("href").Should().Be("/items/42");
        }

        [Theory]
        [InlineData(TargetType.Self, "_self")]
        [InlineData(TargetType.Blank, "_blank")]
        [InlineData(TargetType.Parent, "_parent")]
        [InlineData(TargetType.Top, "_top")]
        public async Task Should_set_the_target_attribute_based_on_target_type(TargetType targetType, string expected)
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.TargetType, targetType));

            link.Find("a").GetAttribute("target").Should().Be(expected);
        }

        [Fact]
        public async Task Should_default_target_type_to_self_when_not_specified()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p.Add(x => x.LinkText, "View"));

            link.Find("a").GetAttribute("target").Should().Be("_self");
        }

        [Fact]
        public async Task Should_default_target_type_to_self_when_the_enum_value_is_not_correct()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p.Add(x => x.LinkText, "View").Add(x => x.TargetType, (TargetType)999));

            link.Find("a").GetAttribute("target").Should().Be("_self");
        }

        [Theory]
        [InlineData(TargetType.Blank, "noopener noreferrer")]
        [InlineData(TargetType.Self, null)]
        [InlineData(TargetType.Parent, null)]
        [InlineData(TargetType.Top, null)]
        public async Task Should_only_set_rel_noopener_noreferrer_when_target_type_is_blank(TargetType targetType, string? expectedRel)
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.TargetType, targetType));

            link.Find("a").GetAttribute("rel").Should().Be(expectedRel);
        }

        [Fact]
        public async Task Should_have_tabindex_zero()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p.Add(x => x.LinkText, "View"));

            link.Find("a").GetAttribute("tabindex").Should().Be("0");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-a-css-variable")]
        public async Task Should_not_apply_the_icon_modifier_class_when_svg_icon_is_null_empty_whitespace_or_invalid(string? svgIcon)
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.SvgIcon, svgIcon));

            link.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}").ClassList
                .Should().NotContain(GlobalValues.Actions_Popover_Action_Icon_Slot_Modifier);
        }

        [Fact]
        public async Task Should_apply_the_icon_modifier_class_and_style_when_svg_icon_is_a_valid_css_variable()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.SvgIcon, "--svg-my-icon"));

            var iconSlot = link.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}");

            using (new AssertionScope())
            {
                iconSlot.ClassList.Should().Contain(GlobalValues.Actions_Popover_Action_Icon_Slot_Modifier);
                iconSlot.GetAttribute("style").Should().Contain("var(--svg-my-icon)");
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_not_add_a_colour_style_when_icon_colour_is_null_empty_or_whitespace(string? iconColour)
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.IconColour, iconColour));

            link.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}").GetAttribute("style")
                .Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Should_add_the_colour_style_when_icon_colour_is_set_even_without_an_svg_icon()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.IconColour, "red"));

            var iconSlot = link.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}");

            using (new AssertionScope())
            {
                iconSlot.ClassList.Should().NotContain(GlobalValues.Actions_Popover_Action_Icon_Slot_Modifier);
                iconSlot.GetAttribute("style").Should().Contain("red");
            }
        }
    }

    public class RaiseOnClick
    {
        [Fact]
        public async Task Should_invoke_on_click_with_the_link_text_item_data_target_type_and_path_when_clicked()
        {
            await using var context = new BunitContext();

            LinkActionData<string>? captured = null;

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.ItemData, "row-42")
                .Add(x => x.TargetType, TargetType.Blank)
                .Add(x => x.Path, "/items/42")
                .Add(x => x.OnClick, (LinkActionData<string> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            link.Find("a").Click();

            using (new AssertionScope())
            {
                captured.Should().NotBeNull();
                captured!.LinkText.Should().Be("View");
                captured.GetValueOr("fallback").Should().Be("row-42");
                captured.TargetType.Should().Be(TargetType.Blank);
                captured.Path.Should().Be("/items/42");
            }
        }

        [Fact]
        public async Task Should_trim_the_link_text_in_the_action_data()
        {
            await using var context = new BunitContext();

            LinkActionData<string>? captured = null;

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "  View  ")
                .Add(x => x.OnClick, (LinkActionData<string> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            link.Find("a").Click();

            captured!.LinkText.Should().Be("View");
        }

        [Fact]
        public async Task Should_report_the_default_path_in_the_action_data_matching_the_rendered_href_when_path_not_supplied()
        {
            await using var context = new BunitContext();

            LinkActionData<string>? captured = null;

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.OnClick, (LinkActionData<string> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            var renderedHref = link.Find("a").GetAttribute("href");

            link.Find("a").Click();

            captured!.Path.Should().Be(renderedHref);
        }

        [Fact]
        public async Task Should_not_throw_when_clicked_and_on_click_has_no_delegate_attached()
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p.Add(x => x.LinkText, "View"));

            var act = () => link.Find("a").Click();

            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_not_throw_when_clicked_regardless_of_prevent_default_value(bool preventDefault)
        {
            await using var context = new BunitContext();

            var link = CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.PreventDefault, preventDefault));

            var act = () => link.Find("a").Click();

            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_not_throw_on_dispose_regardless_of_prevent_default_value(bool preventDefault)
        {
            await using var context = new BunitContext();

            CreateActionPopoverLink<string>(context, p => p
                .Add(x => x.LinkText, "View")
                .Add(x => x.PreventDefault, preventDefault));

            var act = async () => await context.DisposeAsync();

            await act.Should().NotThrowAsync();
        }
    }
}