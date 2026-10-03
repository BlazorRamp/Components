using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Extensions;
using BlazorRamp.Disclosure.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using DisclosureComponent = BlazorRamp.Disclosure.Components.Disclosure;

namespace BlazorRamp.Disclosure.Tests.Unit.Components;

public  class Disclosure_Tests
{
    public const string _coreUtilsModulePath         = "./_content/BlazorRamp.Core/assets/js/core-utilities.js";
    public const string _coreRegisterContainerFunc   = "registerContainerForAutoTabindex";
    public const string _coreUnregisterContainerFunc = "unregisterContainerForAutoTabindex";
    public const string _triggerText                 = "Some trigger text";
    public const string _contentText                 = "Some content text";

    public static IRenderedComponent<DisclosureComponent> CreateDisclosure(BunitContext context, Action<ComponentParameterCollectionBuilder<DisclosureComponent>>? parameters = null)
    {
        // The component injects ICoreUtilityService, so register the real Core services and fake only the JS they call.
        context.Services.AddBlazorRampCore();

        var moduleInterop = context.JSInterop.SetupModule(_coreUtilsModulePath);

        moduleInterop.SetupVoid(_coreRegisterContainerFunc, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(_coreUnregisterContainerFunc, _ => true).SetVoidResult();

        var component = context.Render<DisclosureComponent>(
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
        public async Task Should_throw_null_exception_when_trigger_text_is_null_empty_or_whitespace(string? triggerText)
        {
            await using var context = new BunitContext();

            FluentActions.Invoking(() => CreateDisclosure(context, p => p.Add(x => x.TriggerText, triggerText)))
                          .Should().ThrowExactly<ArgumentNullException>()
                          .WithParameterName("TriggerText")
                          .WithMessage($"*{GlobalValues.Disclosure_Trigger_Text_Exception_Message}*");
                
        }

        [Fact]
        public async Task Should_be_able_to_set_the_trigger_text()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText));

            using(new AssertionScope())
            {
                component.Find($".{GlobalValues.Disclosure_Trigger_Content_Class} > span").TextContent.Should().Be(_triggerText);
                component.Instance.TriggerText.Should().Be(_triggerText);
            }
           
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_content_as_a_region_role(bool isRegion)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentIsRegion, isRegion));

            if(true == isRegion)
            {
                component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("role").Should().Be("region");
                return;
            }

            component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("role").Should().BeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_exclude_or_include_the_trigger_border_and_padding(bool removeBorderPadding)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.RemoveTriggerBorderPadding, removeBorderPadding));

            if(true == removeBorderPadding)
            {
                component.Find("button").ClassList.Should().Contain(GlobalValues.Disclosure_Trigger_Modifier_Class);
                return;
            }

            component.Find("button").ClassList.Should().NotContain(GlobalValues.Disclosure_Trigger_Modifier_Class);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_exclude_or_include_the_content_border_and_padding(bool removeBorderPadding)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.RemoveContentBorderPadding, removeBorderPadding));

            if (true == removeBorderPadding)
            {
                component.Find("div > div").ClassList.Should().Contain(GlobalValues.Disclosure_Content_Modifier_Class);
                return;
            }

            component.Find("div > div").ClassList.Should().NotContain(GlobalValues.Disclosure_Content_Modifier_Class);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_not_apply_a_content_max_height_when_the_value_is_null_empty_or_whitespace(string? maxHeight)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, maxHeight));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("style").Should().BeNull();
        }

        [Theory]
        [InlineData("20rem", "20rem")]
        [InlineData("20em", "20em")]
        [InlineData("20.5rem", "20.5rem")]
        [InlineData(".5rem", ".5rem")]
        [InlineData("10lh", "10lh")]
        [InlineData("40ch", "40ch")]
        [InlineData("300px", "300px")]
        [InlineData("60vh", "60vh")]
        [InlineData("60dvh", "60dvh")]
        [InlineData("60svh", "60svh")]
        [InlineData("60lvh", "60lvh")]
        [InlineData("50vw", "50vw")]
        [InlineData("20REM", "20REM")]
        [InlineData("  20rem  ", "20rem")]
        [InlineData("20rem;", "20rem")]
        public async Task Should_apply_the_content_max_height_as_a_css_variable_when_the_value_is_a_supported_length(string maxHeight, string expectedValue)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, maxHeight));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("style")
                     .Should().Be($"{GlobalValues.Disclosure_Content_Height_Css_Variable_Name}:{expectedValue};");
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("20")]
        [InlineData("rem")]
        [InlineData("20 rem")]
        [InlineData("-5rem")]
        [InlineData("50%")]
        [InlineData("20pt")]
        [InlineData("calc(10rem + 2px)")]
        [InlineData("20rem;position:fixed")]
        public async Task Should_throw_argument_exception_when_the_content_max_height_is_not_a_supported_length(string maxHeight)
        {
            await using var context = new BunitContext();

            FluentActions.Invoking(() => CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, maxHeight)))
                         .Should().ThrowExactly<ArgumentException>()
                         .WithParameterName("ContentMaxHeight")
                         .WithMessage($"*{GlobalValues.Disclosure_Content_Max_Height_Exception_Message}*");
        }

        [Fact]
        public async Task Should_update_the_content_max_height_when_the_value_changes()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, "20rem"));

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, "30rem"));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("style")
                     .Should().Be($"{GlobalValues.Disclosure_Content_Height_Css_Variable_Name}:30rem;");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_remove_the_content_max_height_when_the_value_is_later_cleared(string? clearedValue)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, "20rem"));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("style").Should().NotBeNull();

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.ContentMaxHeight, clearedValue));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").GetAttribute("style").Should().BeNull();
        }

        [Fact]
        public async Task Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                component.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

                component.Find($".{GlobalValues.Disclosure_Class}").GetAttribute("style").Should().Be("color:red;");
            }

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_not_render_the_icon_when_the_svg_icon_is_null_empty_or_whitespace(string? svgIcon)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, svgIcon));

            component.FindAll($".{GlobalValues.Disclosure_Icon_Class}").Should().BeEmpty();
        }

        [Theory]
        [InlineData("svg-my-icon")]
        [InlineData("-svg-my-icon")]
        [InlineData("var(--svg-my-icon)")]
        [InlineData("url(data:image/svg+xml,abc)")]
        public async Task Should_not_render_the_icon_when_the_svg_icon_does_not_begin_with_two_hyphens(string svgIcon)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, svgIcon));

            component.FindAll($".{GlobalValues.Disclosure_Icon_Class}").Should().BeEmpty();
        }

        [Theory]
        [InlineData("--svg-my-icon", "--svg-my-icon")]
        [InlineData("  --svg-my-icon  ", "--svg-my-icon")]
        [InlineData("--svg-my-icon:", "--svg-my-icon")]
        public async Task Should_render_the_icon_using_the_supplied_css_variable(string svgIcon, string expectedVariable)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, svgIcon));

            component.Find($".{GlobalValues.Disclosure_Icon_Class}").GetAttribute("style")
                     .Should().Be($"{GlobalValues.Disclosure_Trigger_Icon_Svg_Css_Variable_Name}:var({expectedVariable});");
        }

        [Fact]
        public async Task Should_hide_the_icon_from_assistive_technology()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, "--svg-my-icon"));

            component.Find($".{GlobalValues.Disclosure_Icon_Class}").GetAttribute("aria-hidden").Should().Be("true");
        }

        [Fact]
        public async Task Should_update_the_icon_when_the_svg_icon_changes()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, "--svg-first-icon"));

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, "--svg-second-icon"));

            component.Find($".{GlobalValues.Disclosure_Icon_Class}").GetAttribute("style")
                     .Should().Be($"{GlobalValues.Disclosure_Trigger_Icon_Svg_Css_Variable_Name}:var(--svg-second-icon);");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_remove_the_icon_when_the_svg_icon_is_later_cleared(string? clearedValue)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, "--svg-my-icon"));

            component.FindAll($".{GlobalValues.Disclosure_Icon_Class}").Should().HaveCount(1);

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.SvgIcon, clearedValue));

            component.FindAll($".{GlobalValues.Disclosure_Icon_Class}").Should().BeEmpty();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_persist_or_remove_the_content_when_collapsed(bool persistContent)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, persistContent)
                                                            .AddChildContent(_contentText));

            var contentText = component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent;

            if (true == persistContent)
            {
                contentText.Should().Contain(_contentText);
                return;
            }

            contentText.Trim().Should().BeEmpty();
        }

        [Fact]
        public async Task Should_persist_the_content_when_collapsed_by_default()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).AddChildContent(_contentText));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Should().Contain(_contentText);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_render_the_content_when_expanded_whether_or_not_it_is_persisted(bool persistContent)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, persistContent)
                                                            .Add(x => x.Expanded, true)
                                                            .AddChildContent(_contentText));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Should().Contain(_contentText);
        }

        [Fact]
        public async Task Should_render_the_content_when_expanded_by_the_user_and_the_content_is_not_persisted()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, false)
                                                            .AddChildContent(_contentText));

            await component.Find("button").ClickAsync();

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Should().Contain(_contentText);
        }

        [Fact]
        public async Task Should_remove_the_content_when_collapsed_by_the_user_and_the_content_is_not_persisted()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, false)
                                                            .Add(x => x.Expanded, true)
                                                            .AddChildContent(_contentText));

            await component.Find("button").ClickAsync();

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Trim().Should().BeEmpty();
        }

        [Fact]
        public async Task Should_keep_the_content_when_collapsed_by_the_user_and_the_content_is_persisted()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, true)
                                                            .Add(x => x.Expanded, true)
                                                            .AddChildContent(_contentText));

            await component.Find("button").ClickAsync();

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Should().Contain(_contentText);
        }

        [Fact]
        public async Task Should_always_render_the_content_element_so_aria_controls_has_a_target()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, false)
                                                            .AddChildContent(_contentText));

            var contentId = component.Find($".{GlobalValues.Disclosure_Content_Class}").Id;

            using (new AssertionScope())
            {
                contentId.Should().NotBeNullOrWhiteSpace();
                component.Find("button").GetAttribute("aria-controls").Should().Be(contentId);
            }
        }

        [Fact]
        public async Task Should_remove_the_collapsed_content_when_persist_content_is_later_turned_off()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.PersistContent, true)
                                                            .AddChildContent(_contentText));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Should().Contain(_contentText);

            component.Render(p => p.Add(x => x.TriggerText, _triggerText)
                                   .Add(x => x.PersistContent, false)
                                   .AddChildContent(_contentText));

            component.Find($".{GlobalValues.Disclosure_Content_Class}").TextContent.Trim().Should().BeEmpty();
        }


        [Fact]
        public async Task Should_be_collapsed_by_default()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText));

            component.Find("button").GetAttribute("aria-expanded").Should().Be("false");
        }

        [Theory]
        [InlineData(true, "true")]
        [InlineData(false, "false")]
        public async Task Should_be_able_to_set_the_initial_expanded_state(bool expanded, string expectedAriaExpanded)
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, expanded));

            component.Find("button").GetAttribute("aria-expanded").Should().Be(expectedAriaExpanded);
        }

        [Fact]
        public async Task Should_toggle_the_expanded_state_when_the_trigger_is_clicked()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText));

            await component.Find("button").ClickAsync();
            component.Find("button").GetAttribute("aria-expanded").Should().Be("true");

            await component.Find("button").ClickAsync();
            component.Find("button").GetAttribute("aria-expanded").Should().Be("false");
        }

        [Fact]
        public async Task Should_follow_the_parent_when_the_expanded_value_changes()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, false));

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, true));
            component.Find("button").GetAttribute("aria-expanded").Should().Be("true");

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, false));
            component.Find("button").GetAttribute("aria-expanded").Should().Be("false");
        }

        [Fact]
        public async Task Should_keep_the_users_toggle_when_the_parent_rerenders_with_an_unchanged_expanded_value()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, false));

            await component.Find("button").ClickAsync();

            component.Render(p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, false));

            component.Find("button").GetAttribute("aria-expanded").Should().Be("true");
        }

        [Fact]
        public async Task Should_invoke_expanded_changed_with_true_when_expanded_by_the_user()
        {
            await using var context = new BunitContext();
            var calls = new List<bool>();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            await component.Find("button").ClickAsync();

            calls.Should().Equal(true);
        }

        [Fact]
        public async Task Should_invoke_expanded_changed_for_each_toggle()
        {
            await using var context = new BunitContext();
            var calls = new List<bool>();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            await component.Find("button").ClickAsync();
            await component.Find("button").ClickAsync();

            calls.Should().Equal(true, false);
        }

        [Fact]
        public async Task Should_invoke_expanded_changed_with_false_when_collapsed_by_the_user()
        {
            await using var context = new BunitContext();
            var calls = new List<bool>();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.Expanded, true)
                                                            .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            await component.Find("button").ClickAsync();

            calls.Should().Equal(false);
        }

        [Fact]
        public async Task Should_not_invoke_expanded_changed_when_the_parent_changes_the_expanded_value()
        {
            await using var context = new BunitContext();
            var calls = new List<bool>();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            component.Render(p => p.Add(x => x.TriggerText, _triggerText)
                                   .Add(x => x.Expanded, true)
                                   .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            calls.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_update_the_expanded_state_when_the_user_toggles_and_expanded_changed_is_bound()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.ExpandedChanged, _ => { }));

            await component.Find("button").ClickAsync();

            component.Find("button").GetAttribute("aria-expanded").Should().Be("true");
        }
    }


    public class SetExpandedState
    {
        [Fact]
        public async Task Should_expand_the_disclosure_when_set_to_true()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText));

            await component.InvokeAsync(() => component.Instance.SetExpandedState(true));

            component.Find("button").GetAttribute("aria-expanded").Should().Be("true");
        }

        [Fact]
        public async Task Should_collapse_the_disclosure_when_set_to_false()
        {
            await using var context = new BunitContext();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText).Add(x => x.Expanded, true));

            await component.InvokeAsync(() => component.Instance.SetExpandedState(false));

            component.Find("button").GetAttribute("aria-expanded").Should().Be("false");
        }

        [Fact]
        public async Task Should_invoke_expanded_changed_when_the_state_changes()
        {
            await using var context = new BunitContext();
            var calls = new List<bool>();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            await component.InvokeAsync(() => component.Instance.SetExpandedState(true));

            calls.Should().Equal(true);
        }

        [Fact]
        public async Task Should_not_invoke_expanded_changed_when_the_state_is_unchanged()
        {
            await using var context = new BunitContext();
            var calls = new List<bool>();

            var component = CreateDisclosure(context, p => p.Add(x => x.TriggerText, _triggerText)
                                                            .Add(x => x.Expanded, true)
                                                            .Add(x => x.ExpandedChanged, value => calls.Add(value)));

            await component.InvokeAsync(() => component.Instance.SetExpandedState(true));

            calls.Should().BeEmpty();
        }
    }
}
