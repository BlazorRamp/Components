using BlazorRamp.ActionsPopover.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using ActionsPopoverComponent = BlazorRamp.ActionsPopover.Components.ActionsPopover;

namespace BlazorRamp.ActionsPopover.Tests.Unit.Components;

public class ActionsPopover_Tests
{
    public static IRenderedComponent<ActionsPopoverComponent> CreateActionsPopover(
    BunitContext context,
    Action<ComponentParameterCollectionBuilder<ActionsPopoverComponent>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Register_Focus_Out_Handler, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Unregister_Focus_Out_Handler, _ => true).SetVoidResult();

        var component = context.Render<ActionsPopoverComponent>(
            builder =>
            {
                parameters?.Invoke(builder);
            });

        return component;
    }
    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My Actions")]
        public async Task Should_set_the_trigger_text_which_defaults_if_null_empty_or_whitespace(string? triggerText)
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context, p => p.Add(x => x.TriggerText, triggerText));

            var triggerTextSpan = popover.Find($"button.{GlobalValues.Actions_Popover_Trigger_Class} span[id]");

            if (String.IsNullOrWhiteSpace(triggerText))
            {
                triggerTextSpan.TextContent.Should().Be(GlobalValues.Actions_Popover_Trigger_Text);
                return;
            }
            triggerTextSpan.TextContent.Should().Be(triggerText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-a-css-variable")]
        public async Task Should_not_apply_the_icon_modifier_class_when_svg_icon_is_null_empty_whitespace_or_invalid(string? svgIcon)
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context, p => p.Add(x => x.SvgIcon, svgIcon));

            var iconSlot = popover.Find($".{GlobalValues.Actions_Popover_Trigger_Icon_Slot_Class}");

            using (new AssertionScope())
            {
                iconSlot.ClassList.Should().NotContain(GlobalValues.Actions_Popover_Trigger_Icon_Slot_Modifier);
                iconSlot.GetAttribute("style").Should().BeNullOrEmpty();
            }
        }
        [Fact]
        public async Task Should_apply_the_icon_modifier_class_and_style_when_svg_icon_is_a_valid_css_variable()
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context, p => p.Add(x => x.SvgIcon, "--svg-my-icon"));

            var iconSlot = popover.Find($".{GlobalValues.Actions_Popover_Trigger_Icon_Slot_Class}");

            using (new AssertionScope())
            {
                iconSlot.ClassList.Should().Contain(GlobalValues.Actions_Popover_Trigger_Icon_Slot_Modifier);
                iconSlot.GetAttribute("style").Should().Contain("var(--svg-my-icon)");
            }
        }

        [Theory]
        [InlineData(ActionsPopoverPosition.TopCentre, "top-centre")]
        [InlineData(ActionsPopoverPosition.TopLeft, "top-left")]
        [InlineData(ActionsPopoverPosition.TopRight, "top-right")]
        [InlineData(ActionsPopoverPosition.CentreLeft, "centre-left")]
        [InlineData(ActionsPopoverPosition.CentreRight, "centre-right")]
        [InlineData(ActionsPopoverPosition.BottomCentre, "bottom-centre")]
        [InlineData(ActionsPopoverPosition.BottomLeft, "bottom-left")]
        [InlineData(ActionsPopoverPosition.BottomRight, "bottom-right")]
        public async Task Should_set_the_popover_position_data_attribute(ActionsPopoverPosition position, string expected)
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context, p => p.Add(x => x.ActionsPopoverPosition, position));

            popover.Find($".{GlobalValues.Actions_Popover_Panel_Class}")
                   .GetAttribute("data-br-actions-popover-position")
                   .Should().Be(expected);
        }

        [Fact]
        public async Task Should_default_the_popover_position_to_bottom_left_when_not_specified()
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context);

            popover.Find($".{GlobalValues.Actions_Popover_Panel_Class}")
                   .GetAttribute("data-br-actions-popover-position")
                   .Should().Be("bottom-left");
        }

        [Fact]
        public async Task Should_default_the_popover_position_to_bottom_left_with_incorrect_enum_value()
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context, p => p.Add(x => x.ActionsPopoverPosition, (ActionsPopoverPosition)999));

            popover.Find($".{GlobalValues.Actions_Popover_Panel_Class}")
                   .GetAttribute("data-br-actions-popover-position")
                   .Should().Be("bottom-left");
        }

        [Fact]
        public async Task Should_link_the_trigger_button_to_the_popover_panel_via_popovertarget()
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context);

            var triggerPopoverTarget = popover.Find($"button.{GlobalValues.Actions_Popover_Trigger_Class}").GetAttribute("popovertarget");
            var panelID = popover.Find($".{GlobalValues.Actions_Popover_Panel_Class}").GetAttribute("id");

            triggerPopoverTarget.Should().Be(panelID);
        }

        [Fact]
        public async Task Should_render_the_popover_items_inside_the_panel()
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context, p => p.Add(
                x => x.PopoverItems,
                builder =>
                {
                    builder.OpenElement(0, "li");
                    builder.AddContent(1, "Test Item");
                    builder.CloseElement();
                }));

            popover.Find($".{GlobalValues.Actions_Popover_Panel_Class} ul").TextContent.Should().Contain("Test Item");
        }


        [Fact]
        public async Task Should_not_throw_when_disposed()
        {
            await using var context = new BunitContext();

            var popover = CreateActionsPopover(context);

            var act = async () => await context.DisposeAsync();

            await act.Should().NotThrowAsync();
        }



    }
}
