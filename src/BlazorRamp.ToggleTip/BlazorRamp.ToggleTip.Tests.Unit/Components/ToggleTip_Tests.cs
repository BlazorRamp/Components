using Bunit;
using FluentAssertions;
using BlazorRamp.ToggleTip.Common.Constants;
using ToggleTipComponent = BlazorRamp.ToggleTip.Components.ToggleTip;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.ToggleTip.Tests.Unit.Components;

public class ToggleTip_Tests
{
    public static IRenderedComponent<ToggleTipComponent> CreateToggleTipWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, bool showLabel = true, bool showCloseButton = true)

            => context.Render<ToggleTipComponent>(paramBuilder => paramBuilder.Add(p => p.ShowLabel, showLabel)
                                                                              .Add(p => p.ShowClose, showCloseButton) 
                                                                              .TryAdd<TValue>(paramName, paramValue));

    public class Parameters()
    {
        [Fact]
        public void Should_be_able_to_set_the_label_param()
        {
            using var context = new BunitContext();

            var toggleTipComponent = context.Render<ToggleTipComponent>(paramBuilder => paramBuilder.Add<string>(p => p.Label, "Keyboard:"));

            toggleTipComponent.Find("div > button > span:first-child").TextContent.Should().Be("Keyboard:");
        }
        [Theory]
        [InlineData("missing")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void The_default_label_value_should_be_used_if_missing_null_whitespace_or_empty(string? labelValue)
        {
            using var context = new BunitContext();

            IRenderedComponent<ToggleTipComponent> toggleTipComponent;

            if (labelValue == "missing")
            {
                toggleTipComponent = context.Render<ToggleTipComponent>();
            }
            else
            {
                toggleTipComponent = CreateToggleTipWithParamByName<string>(context, nameof(ToggleTipComponent.Label), labelValue!);
            }
            toggleTipComponent.Find("div > button > span:first-child").TextContent.Should().Be(GlobalValues.ToggleTip_Label);
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void The_label_param_should_set_the_aria_label_on_the_button(bool showLabel)
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<string>(context, nameof(ToggleTipComponent.Label), "Keyboard:", showLabel);

            toggleTipComponent.Find("div > button").GetAttribute("aria-label").Should().Be("Keyboard:");
        }

        [Fact]
        public void Should_be_able_to_set_the_close_text_param()
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<string>(context, nameof(ToggleTipComponent.CloseText), "Close Text");

            toggleTipComponent.Find($"button.{@GlobalValues.ToggleTip_Closer_Class}").TextContent.Should().Be("Close Text");
        }
        [Theory]
        [InlineData("missing")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void The_default_close_text_should_be_used_if_the_close_param_is_missing_null_whitespace_or_empty(string? closeText)
        {
            using var context = new BunitContext();

            IRenderedComponent<ToggleTipComponent> toggleTipComponent;

            if (closeText == "missing")
            {
                toggleTipComponent = context.Render<ToggleTipComponent>();
            }
            else
            {
                toggleTipComponent = CreateToggleTipWithParamByName<string>(context, nameof(ToggleTipComponent.CloseText), closeText!);
            }
            toggleTipComponent.Find($"button.{GlobalValues.ToggleTip_Closer_Class}").TextContent.Should().Be(GlobalValues.ToggleTip_Close_Text);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_hide_the_close_button_by_setting_the_show_close_param_to_false(bool showClose)
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<string>(context, nameof(ToggleTipComponent.CloseText), "Close Text", true,showCloseButton: showClose);

            if (showClose)
            {
                toggleTipComponent.FindAll($"button.{@GlobalValues.ToggleTip_Closer_Class}").Should().HaveCount(1);
            }
            else
            {
                toggleTipComponent.FindAll($"button.{@GlobalValues.ToggleTip_Closer_Class}").Should().BeEmpty();
            }

        }

        [Theory]
        [InlineData(ToggleTipLabelOrder.LabelFirst)]
        [InlineData(ToggleTipLabelOrder.IconFirst)]
        public void Should_be_able_to_switch_label_and_icon_order_using_the_toggle_tip_label_order_param(ToggleTipLabelOrder lableOrder)
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<ToggleTipLabelOrder>(context, nameof(ToggleTipComponent.ToggleTipLabelOrder), lableOrder);

            var classList = toggleTipComponent.Find("div > button").ClassList;

            if (lableOrder == ToggleTipLabelOrder.IconFirst)
            {
                classList.Should().Contain(GlobalValues.ToggleTip_Trigger_Order_Modifier_Class);
            }
            else
            {
                classList.Should().NotContain(GlobalValues.ToggleTip_Trigger_Order_Modifier_Class);
            }
        }

        [Theory]
        [InlineData(ToggleTipSize.Small, GlobalValues.ToggleTip_Small_Modifier_Class)]
        [InlineData(ToggleTipSize.Medium,GlobalValues.ToggleTip_medium_Modifier_Class)]
        [InlineData(ToggleTipSize.Large, GlobalValues.ToggleTip_large_Modifier_Class)]
        public void Should_be_able_to_set_the_size_of_the_toggle_tip_using_the_toggle_tip_size_param(ToggleTipSize toggleTipSize, string className)
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<ToggleTipSize>(context, nameof(ToggleTipComponent.ToggleTipSize), toggleTipSize);

            var classList = toggleTipComponent.Find("div").ClassList;

            classList.Should().Contain(className);
        }

        [Fact]
        public void Should_use_a_default_size_if_the_toggle_tip_size_enum_is_not_matched()
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<ToggleTipSize>(context, nameof(ToggleTipComponent.ToggleTipSize), (ToggleTipSize)99);

            var classList = toggleTipComponent.Find("div").ClassList;

            classList.Should().Contain(GlobalValues.ToggleTip_Small_Modifier_Class);
        }

        [Theory]
        [InlineData(ToggleTipPosition.TopCentre,"top-centre")]
        [InlineData(ToggleTipPosition.TopLeft, "top-left")]
        [InlineData(ToggleTipPosition.TopRight, "top-right")]
        [InlineData(ToggleTipPosition.CentreLeft, "centre-left")]
        [InlineData(ToggleTipPosition.CentreRight, "centre-right")]
        [InlineData(ToggleTipPosition.BottomCentre, "bottom-centre")]
        [InlineData(ToggleTipPosition.BottomLeft, "bottom-left")]
        [InlineData(ToggleTipPosition.BottomRight, "bottom-right")]
        public void Should_be_able_to_set_the_postion_of_the_toggle_tip_using_the_toggle_tip_postion_param(ToggleTipPosition toggleTipPosition, string attribueValue)
        {
            using var context = new BunitContext();

            var toggleTipComponent = CreateToggleTipWithParamByName<ToggleTipPosition>(context, nameof(ToggleTipComponent.ToggleTipPosition), toggleTipPosition);

            var dataAttribute = toggleTipComponent.Find("div > div").GetAttribute("data-br-toggle-tip-position");

            dataAttribute.Should().Be(attribueValue);   
        }


        [Fact]
        public void Should_be_able_to_set_the_render_fragment_param()
        {
            using var context = new BunitContext();

            var toggleTipComponent = context.Render<ToggleTipComponent>(parameters => parameters.AddChildContent("<h1>Test Content</h1>"));

            toggleTipComponent.Find($"div.{GlobalValues.ToggleTip_Content_Area_Class}").InnerHtml.Should().Be("<h1>Test Content</h1>");
        }


        [Fact]
        public void Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            using var context = new BunitContext();

            var toggleTipComponent = context.Render<ToggleTipComponent>(parameters => parameters.AddUnmatched("style", "color:red;"));

            toggleTipComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");
        }
        
    }

    public class GetToggleTiPositionFromEnum()
    {
        [Theory]
        [InlineData(ToggleTipPosition.TopCentre, "top-centre")]
        [InlineData(ToggleTipPosition.TopLeft, "top-left")]
        [InlineData(ToggleTipPosition.TopRight, "top-right")]
        [InlineData(ToggleTipPosition.CentreLeft, "centre-left")]
        [InlineData(ToggleTipPosition.CentreRight, "centre-right")]
        [InlineData(ToggleTipPosition.BottomCentre, "bottom-centre")]
        [InlineData(ToggleTipPosition.BottomLeft, "bottom-left")]
        [InlineData(ToggleTipPosition.BottomRight, "bottom-right")]
        public void The_internal_method_should_return_the_correct_attribute_value_for_the_enum(ToggleTipPosition toggleTipPosition, string attributeValue)
        
            => _ = new ToggleTipComponent().GetToggleTipPositionFromEnum(toggleTipPosition).Should().Be(attributeValue);
        
        [Fact]
        public void The_internal_method_should_return_a_default_if_an_unmatched_enum_value_is_used()
        
            => _ = new ToggleTipComponent().GetToggleTipPositionFromEnum((ToggleTipPosition)99).Should().Be("top-centre");



    }

}
