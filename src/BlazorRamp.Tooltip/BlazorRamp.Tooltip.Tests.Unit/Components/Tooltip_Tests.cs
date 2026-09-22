using BlazorRamp.Tooltip.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using TooltipComponent = BlazorRamp.Tooltip.Components.Tooltip;


namespace BlazorRamp.Tooltip.Tests.Unit.Components;

public class Tooltip_Tests
{
    public const string _tooltipID = "my-tooltip-id";
    public const string _tooltipText = "Some tooltip text";

    public static IRenderedComponent<TooltipComponent> CreateTooltip(BunitContext context, Action<ComponentParameterCollectionBuilder<TooltipComponent>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Register_Tooltip_Func, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Unregister_Tooltip_Func, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Close_Open_Tooltips_Func).SetVoidResult();

        var component = context.Render<TooltipComponent>(
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
        public async Task Should_throw_argument_null_exception_when_tooltip_id_is_null_empty_or_whitespace(string? tooltipId)
        {
            await using var context = new BunitContext();

            FluentActions.Invoking(() => CreateTooltip(context, p => p.Add(x => x.TooltipID, tooltipId).Add(x => x.TooltipText, _tooltipText)))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName(nameof(TooltipComponent.TooltipID))
                .WithMessage($"*{GlobalValues.Tooltip_ID_Exception_Message}*");
        }

    }
    [Fact]
    public async Task Should_set_the_tooltip_id_when_a_value_is_provided()
    {
        await using var context = new BunitContext();
        var tooltipComponent = CreateTooltip(context, p => p.Add(x => x.TooltipID, _tooltipID).Add(x => x.TooltipText, _tooltipText));

        using(new AssertionScope())
        {
            tooltipComponent.Find($"#{_tooltipID}").Should().NotBeNull();
            tooltipComponent.Instance.TooltipText.Should().Be(_tooltipText);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Should_throw_argument_null_exception_when_tooltip_text_is_null_empty_or_whitespace(string? tooltipText)
    {
        await using var context = new BunitContext();

        FluentActions.Invoking(() => CreateTooltip(context, p => p.Add(x => x.TooltipID, _tooltipID).Add(x => x.TooltipText, tooltipText)))
            .Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(nameof(TooltipComponent.TooltipText))
            .WithMessage($"*{GlobalValues.Tooltip_Text_Exception_Message}*");
    }

    [Fact]
    public async Task Should_be_able_to_set_the_tooltip_text()
    {
        await using var context = new BunitContext();
        var tooltipComponent = CreateTooltip(context, p => p.Add(x => x.TooltipID, _tooltipID).Add(x => x.TooltipText, _tooltipText));

        using (new AssertionScope())
        {
            tooltipComponent.Find($".{GlobalValues.Tooltip_Content_class}").TextContent.Trim().Should().Be(_tooltipText);
            tooltipComponent.Instance.TooltipText.Should().Be(_tooltipText);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Should_be_able_to_use_inverted_colours_for_the_tooltip(bool useInvertedColours)
    {
        await using var context = new BunitContext();

        var tooltipComponent = CreateTooltip(context, p => p.Add(x => x.TooltipID, _tooltipID).Add(x => x.TooltipText, _tooltipText).Add(x => x.InvertColours, useInvertedColours));

        var classList = tooltipComponent.Find($"div.{GlobalValues.Tooltip_Content_Area_class}").ClassList;

        if(true == useInvertedColours)
        {
            classList.Should().Contain(GlobalValues.Tooltip_Content_Area_Modifier);
            return;
        }

        classList.Should().NotContain(GlobalValues.Tooltip_Content_Area_Modifier);
    }

    [Theory]
    [InlineData(TooltipPosition.TopCentre, "top-centre")]
    [InlineData(TooltipPosition.TopLeft, "top-left")]
    [InlineData(TooltipPosition.TopRight, "top-right")]
    [InlineData(TooltipPosition.CentreLeft, "centre-left")]
    [InlineData(TooltipPosition.CentreRight, "centre-right")]
    [InlineData(TooltipPosition.BottomCentre, "bottom-centre")]
    [InlineData(TooltipPosition.BottomLeft, "bottom-left")]
    [InlineData(TooltipPosition.BottomRight, "bottom-right")]
    public async Task Should_set_the_tooltip_position_data_attribute(TooltipPosition position, string expected)
    {
        await using var context = new BunitContext();

        var tooltipComponent = CreateTooltip(context, p => p.Add(x => x.TooltipID,_tooltipID).Add(x => x.TooltipText, _tooltipText).Add(x => x.TooltipPosition, position));

        tooltipComponent.Find($".{GlobalValues.Tooltip_Content_Wrapper_class}").GetAttribute("data-br-tooltip-position").Should().Be(expected);
    }

    [Fact]
    public async Task Should_default_the_tooltip_position_to_top_centre_with_incorrect_enum_value()
    {
        await using var context = new BunitContext();

        var tooltipComponent = CreateTooltip(context, p => p.Add(x => x.TooltipID, _tooltipID).Add(x => x.TooltipText, _tooltipText).Add(x => x.TooltipPosition, (TooltipPosition)99));

        tooltipComponent.Find($".{GlobalValues.Tooltip_Content_Wrapper_class}").GetAttribute("data-br-tooltip-position").Should().Be("top-centre");
    }

    [Fact]
    public async Task Should_call_close_open_tooltips_when_the_closer_is_clicked()
    {
        await using var context = new BunitContext();

        var tooltipComponent = CreateTooltip(context, p => p.Add(x => x.TooltipID, _tooltipID).Add(x => x.TooltipText, _tooltipText));

        await tooltipComponent.Find($".{GlobalValues.Tooltip_Closer_Class}").ClickAsync();

        context.JSInterop.VerifyInvoke(GlobalValues.JS_Close_Open_Tooltips_Func);
    }
}
