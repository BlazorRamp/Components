using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using BlazorRamp.ActionsPopover.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.ActionsPopover.Tests.Unit.Components;

public class ActionPopoverButton_Tests
{
    public static IRenderedComponent<ActionPopoverButton<TData>> CreateActionPopoverButton<TData>(
        BunitContext context,
        Action<ComponentParameterCollectionBuilder<ActionPopoverButton<TData>>>? parameters = null)
    {
        var component = context.Render<ActionPopoverButton<TData>>(
            builder => parameters?.Invoke(builder));

        return component;
    }

    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_throw_when_button_text_is_null_empty_or_whitespace(string? buttonText)
        {
            await using var context = new BunitContext();

            var act = () => CreateActionPopoverButton<string>(context, p => p.Add(x => x.ButtonText, buttonText!));

            act.Should().Throw<ArgumentNullException>()
               .WithMessage($"*{GlobalValues.Actions_Popover_Button_Text_Exception_Message}*");
        }

        [Fact]
        public async Task Should_render_the_button_text()
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context, p => p.Add(x => x.ButtonText, "Edit"));

            button.Find($".{GlobalValues.Actions_Popover_Action_Text_Class}").TextContent.Trim().Should().Be("Edit");
        }

        [Fact]
        public async Task Should_set_the_popovertarget_attribute_from_the_cascaded_popover_id()
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .AddCascadingValue(GlobalValues.Actions_Popover_Panel_Cascading_ID_Name, "test-popover-id"));

            button.Find("button").GetAttribute("popovertarget").Should().Be("test-popover-id");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-a-css-variable")]
        public async Task Should_not_apply_the_icon_modifier_class_when_svg_icon_is_null_empty_whitespace_or_invalid(string? svgIcon)
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.SvgIcon, svgIcon));

            button.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}").ClassList
                  .Should().NotContain(GlobalValues.Actions_Popover_Action_Icon_Slot_Modifier);
        }

        [Fact]
        public async Task Should_apply_the_icon_modifier_class_and_style_when_svg_icon_is_a_valid_css_variable()
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.SvgIcon, "--svg-my-icon"));

            var iconSlot = button.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}");

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

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.IconColour, iconColour));

            button.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}").GetAttribute("style")
                  .Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Should_add_the_colour_style_when_icon_colour_is_set_even_without_an_svg_icon()
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.IconColour, "red"));

            var iconSlot = button.Find($".{GlobalValues.Actions_Popover_Action_Icon_Slot_Class}");

            using (new AssertionScope())
            {
                iconSlot.ClassList.Should().NotContain(GlobalValues.Actions_Popover_Action_Icon_Slot_Modifier);
                iconSlot.GetAttribute("style").Should().Contain("red");
            }
        }
        [Fact]
        public async Task Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context,p => p.Add(x => x.ButtonText, "My Button").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                button.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

                var styleAttrib = button.Find("button").GetAttribute("style");

                styleAttrib.Should().Be("color:red;");

            }

        }
    }

    public class RaiseOnClick
    {
        [Fact]
        public async Task Should_invoke_on_click_with_the_button_text_and_item_data_when_clicked()
        {
            await using var context = new BunitContext();

            ButtonActionData<string>? captured = null;

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.ItemData, "row-42")
                .Add(x => x.OnClick, (ButtonActionData<string> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            button.Find("button").Click();

            using (new AssertionScope())
            {
                captured.Should().NotBeNull();
                captured!.ButtonText.Should().Be("Edit");
                captured.GetValueOr("fallback").Should().Be("row-42");
            }
        }

        [Fact]
        public async Task Should_trim_the_button_text_in_the_action_data()
        {
            await using var context = new BunitContext();

            ButtonActionData<string>? captured = null;

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "  Edit  ")
                .Add(x => x.OnClick, (ButtonActionData<string> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            button.Find("button").Click();

            captured!.ButtonText.Should().Be("Edit");
        }

        

        [Fact]
        public async Task Should_not_throw_when_clicked_and_on_click_has_no_delegate_attached()
        {
            await using var context = new BunitContext();

            var button = CreateActionPopoverButton<string>(context, p => p.Add(x => x.ButtonText, "Edit"));

            var act = () => button.Find("button").Click();

            act.Should().NotThrow();
        }


        [Fact]
        public async Task Should_return_the_fallback_from_get_value_or_when_no_item_data_was_supplied_for_a_reference_type()
        {
            await using var context = new BunitContext();

            ButtonActionData<string>? captured = null;

            var button = CreateActionPopoverButton<string>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.OnClick, (ButtonActionData<string> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            button.Find("button").Click();

            captured!.GetValueOr("fallback").Should().Be("fallback");
        }

        [Fact]
        public async Task Should_return_the_fallback_from_get_value_or_when_no_item_data_was_supplied_for_a_nullable_value_type()
        {
            await using var context = new BunitContext();

            ButtonActionData<int?>? captured = null;

            var button = CreateActionPopoverButton<int?>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.OnClick, (ButtonActionData<int?> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            button.Find("button").Click();

            captured!.GetValueOr(-1).Should().Be(-1);
        }

        [Fact]
        public async Task Should_return_the_supplied_item_data_from_get_value_or_for_a_nullable_value_type_including_zero()
        {
            await using var context = new BunitContext();

            ButtonActionData<int?>? captured = null;

            var button = CreateActionPopoverButton<int?>(context, p => p
                .Add(x => x.ButtonText, "Edit")
                .Add(x => x.ItemData, 0)
                .Add(x => x.OnClick, (ButtonActionData<int?> data) =>
                {
                    captured = data;
                    return Task.CompletedTask;
                }));

            button.Find("button").Click();

            captured!.GetValueOr(-1).Should().Be(0);
        }



    }

}