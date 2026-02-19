using BlazorRamp.Switch.Common.Constants;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection.Emit;
using SwitchComponent = BlazorRamp.Switch.Components.Switch;

namespace BlazorRamp.Switch.Tests.Unit.Components.Switch;

public class Switch_Tests
{        
    private static IRenderedComponent<SwitchComponent> CreateSwitchWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, string label = "label")

            => context.Render<SwitchComponent>(paramBuilder => paramBuilder.Add<string>(p => p.Label, label).TryAdd(paramName, paramValue));
    
    public class Parameters()
    {


        [Fact]
        public void Should_be_able_to_set_the_label_param()
        {
            using var context = new BunitContext();

            var switchComponent = context.Render<SwitchComponent>(paramBuilder => paramBuilder.Add<string>(p => p.Label, "Switch_Label"));

            switchComponent.Find("button > span:first-child").TextContent.Should().Be("Switch_Label");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_set_the_switch_state_param(bool switchState)
        {
            using var context = new BunitContext();

            var switchComponent = CreateSwitchWithParamByName<bool>(context, nameof(SwitchComponent.SwitchState),switchState);

            switchComponent.Find("button").GetAttribute("aria-checked").Should().Be(switchState.ToString().ToLower());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_set_the_aria_disabled_param(bool ariaDisabled)
        {
            using var context = new BunitContext();

            var switchComponent = CreateSwitchWithParamByName<bool>(context, nameof(SwitchComponent.AriaDisabled), ariaDisabled);

            switchComponent.Find("button").GetAttribute("aria-disabled").Should().Be(ariaDisabled.ToString().ToLower());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_set_the_space_between_param(bool spaceBetween)
        {
            using var context = new BunitContext();

            var switchComponent = CreateSwitchWithParamByName<bool>(context, nameof(SwitchComponent.SpaceBetween), spaceBetween);

            var classList = switchComponent.Find("button").ClassList;

            if(true == spaceBetween)
            {
                classList.Should().Contain(GlobalValues.Switch_Space_Modifier_Class);
                return;
            }

            classList.Should().NotContain(GlobalValues.Switch_Space_Modifier_Class);
        }

        [Fact]
        public void Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            using var context = new BunitContext();

            var switchComponent = context.Render<SwitchComponent>(paramBuilder => paramBuilder.Add(p => p.Label, "Switch_Label")
                                                                  .AddUnmatched("style", "color:red;"));

            switchComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");
        }

    }

    public class OnParametersSet()
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_throw_an_exception_if_the_label_is_null_empty_or_whitespace(string? label)
        {
            using var context = new BunitContext();

            FluentActions.Invoking(() => context.Render<SwitchComponent>(param => param.Add(p => p.Label, label))).Should().Throw<ArgumentNullException>();
        }
    }

    public class RaiseOnSwitchStateChanged()
    {
        [Fact]
        public async Task Should_invoke_callback_when_state_changes()
        {
            using var context  = new BunitContext();
            bool? receivedState = null;

            var switchComponent = context.Render<SwitchComponent>(param => param.Add(p => p.Label, "My Switch")
                                    .Add(p => p.SwitchState, false)
                                    .Add(p => p.AriaDisabled, false)
                                    .Add(p => p.SwitchStateChanged, EventCallback.Factory.Create<bool>(this, (val) => receivedState = val)));

            await switchComponent.InvokeAsync(() => switchComponent.Render(param => param.Add(p => p.SwitchState, true)));

            receivedState.Should().BeTrue();
        }
        [Fact]
        public async Task Should_not_invoke_the_callback_when_state_changes_when_aria_disable_is_true()
        {
            using var context = new BunitContext();
            bool callbackInvoked = false;

            var switchComponent = context.Render<SwitchComponent>(param => param.Add(p => p.Label, "My Switch")
                                    .Add(p => p.SwitchState, false)
                                    .Add(p => p.AriaDisabled, true)
                                    .Add(p => p.SwitchStateChanged, EventCallback.Factory.Create<bool>(this, (val) => callbackInvoked = true)));

            await switchComponent.InvokeAsync(() => switchComponent.Render(param => param.Add(p => p.SwitchState, true)));

            callbackInvoked.Should().BeFalse();
        }

        [Fact]
        public async Task Should_not_invoke_the_callback_when_state_has_not_changed()
        {
            using var context = new BunitContext();
            bool callbackInvoked = false;

            var switchComponent = context.Render<SwitchComponent>(param => param.Add(p => p.Label, "My Switch")
                                    .Add(p => p.SwitchState, false)
                                    .Add(p => p.AriaDisabled, false)
                                    .Add(p => p.SwitchStateChanged, EventCallback.Factory.Create<bool>(this, (val) => callbackInvoked = true)));

            await switchComponent.InvokeAsync(() => switchComponent.Render(param => param.Add(p => p.SwitchState, false)));

            callbackInvoked.Should().BeFalse();
        }

        [Fact]
        public async Task Should_not_invoke_the_callback_when_state_has_not_changed_and_aria_disabled_is_true()
        {
            using var context = new BunitContext();
            bool? receivedState = null;

            var switchComponent = context.Render<SwitchComponent>(param => param
                .Add(p => p.Label, "My Switch")
                .Add(p => p.SwitchState, false)
                .Add(p => p.AriaDisabled, true)
                .Add(p => p.SwitchStateChanged, EventCallback.Factory.Create<bool>(this, (val) => receivedState = val)));

            await switchComponent.InvokeAsync(() => switchComponent.Render(param => param
                .Add(p => p.SwitchState, false)));

            receivedState.Should().BeNull();
        }
    }

    [Fact]
    public async Task Should_invoke_callback_when_the_buton_is_clicked()
    {
        using var context   = new BunitContext();
        bool? receivedState = null;

        var switchComponent = context.Render<SwitchComponent>(param => param
                .Add(p => p.Label, "My Switch")
                .Add(p => p.SwitchState, false)
                .Add(p => p.AriaDisabled, false)
                .Add(p => p.SwitchStateChanged, EventCallback.Factory.Create<bool>(this, (val) => receivedState = val)));

        
        await switchComponent.Find("button").ClickAsync(new MouseEventArgs());
        receivedState.Should().BeTrue();
    }
}
