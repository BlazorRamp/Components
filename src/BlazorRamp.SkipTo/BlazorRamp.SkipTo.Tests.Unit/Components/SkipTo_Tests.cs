using BlazorRamp.SkipTo.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SkipToComponent = BlazorRamp.SkipTo.Components.SkipTo;

namespace BlazorRamp.SkipTo.Tests.Unit.Components;


public class SkipTo_Tests
{

    public class Parameters
    {
        private static IRenderedComponent<SkipToComponent> CreateSkipToWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
        {
            context.JSInterop.SetupModule(GlobalValues.JS_SkipTo_File_Path);

            return context.Render<SkipToComponent>(paramBuilder => paramBuilder.TryAdd(paramName, paramValue));
        }
        private static IRenderedComponent<SkipToComponent> CreateSkipToWithoutParams(BunitContext context)
        {
            context.JSInterop.SetupModule(GlobalValues.JS_SkipTo_File_Path);

            return context.Render<SkipToComponent>();
        }

        [Theory]
        [InlineData("Skip Text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Skip_to_text_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? skiptoText)
        {
            await using var context = new BunitContext();
            IRenderedComponent<SkipToComponent> skipTo;

            skipTo = (skiptoText == "missing") ? CreateSkipToWithoutParams(context)
                                               : CreateSkipToWithParamByName<string>(context, nameof(SkipToComponent.SkipToText), skiptoText!);

            var contextText = skipTo.Find($"a.{GlobalValues.SkipTo_Class} > span:last-child").TextContent;

            switch (skiptoText)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(GlobalValues.SkipTo_Text);
                    break;
                default:
                    contextText.Should().Be(skiptoText!.Trim());
                    break;
            }
        }

        [Theory]
        [InlineData("#some-target")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Target_id_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? targetID)
        {
            await using var context = new BunitContext();
            IRenderedComponent<SkipToComponent> skipTo;

            skipTo = (targetID == "missing") ? CreateSkipToWithoutParams(context)
                                               : CreateSkipToWithParamByName<string>(context, nameof(SkipToComponent.TargetID), targetID!);

            var targetHref = skipTo.Find($"a.{GlobalValues.SkipTo_Class}").GetAttribute("href");

            switch (targetID)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    targetHref.Should().Be("#" + GlobalValues.SkipTo_Target_ID);
                    break;
                default:
                    targetHref.Should().Be(targetID!.Trim());
                    break;
            }
        }

        [Theory]
        [InlineData(SkipToType.Section)]
        [InlineData(SkipToType.Site)]
        public async Task Skip_to_type_is_used_to_set_the_correct_classes_for_the_skip_to_anchor(SkipToType skipToType)
        {
            await using var context = new BunitContext();
            IRenderedComponent<SkipToComponent> skipTo;

            skipTo = CreateSkipToWithParamByName<SkipToType>(context, nameof(SkipToComponent.SkipToType),skipToType);

            var classValues = skipTo.Find($"a").ClassName;

            var skipToClasses = skipToType == SkipToType.Section ? $"{GlobalValues.SkipTo_Class} {GlobalValues.SkipTo_Container_Modifier}" :  GlobalValues.SkipTo_Class;

            classValues.Should().Be(skipToClasses);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Icon_visible_should_determine_if_the_icon_is_shown(bool iconVisible)
        {
            await using var context = new BunitContext();
            IRenderedComponent<SkipToComponent> skipTo;

            skipTo = CreateSkipToWithParamByName<bool>(context, nameof(SkipToComponent.IconVisible), iconVisible!);

            var iconClass = skipTo.Find($"a > span.{GlobalValues.SkipTo_Content_Class}").FirstElementChild?.ClassName ?? String.Empty;

            if(true == iconVisible)
            {
                iconClass.Should().Be(GlobalValues.SkipTo_Icon_Class);
            }
            else
            {
                iconClass.Should().BeEmpty();
            }
        }
    }

    public class HandleNavigation()
    {
        [Fact]
        public async Task Should_navigate_and_invoke_the_javascript_function()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_SkipTo_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_SkipTo_Scroll_Focus_Func, _ => true);

            var navManager = context.Services.GetRequiredService<NavigationManager>();

            var skipTo = context.Render<SkipToComponent>(paramBuilder => paramBuilder.Add(p => p.TargetID, "main-content"));

            skipTo.Find("a").Click();

            using(new AssertionScope())
            {
                navManager.Uri.Should().Contain("main-content");
                moduleInterop.VerifyInvoke(GlobalValues.JS_SkipTo_Scroll_Focus_Func).Arguments[0].Should().Be("main-content");
            }

        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_not_navigate_if_the_url_navigate_to_is_null_empty_or_whitespace(string? href)
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_SkipTo_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_SkipTo_Scroll_Focus_Func, _ => true);

            var navManager = context.Services.GetRequiredService<NavigationManager>();

            var skipTo = context.Render<SkipToComponent>(paramBuilder => paramBuilder.Add(p => p.TargetID, "main-content"));

            var navigationHandler = skipTo.Instance.GetType().GetMethod("HandleNavigation", BindingFlags.NonPublic | BindingFlags.Instance);

            var currentLocation = navManager.Uri;

            await (Task)navigationHandler!.Invoke(skipTo.Instance, new object[] { href!, "main-content" })!;

            navManager.Uri.Should().Be(currentLocation);

        }
    }

    public class NavigationManager_LocationChanged()
    {
        [Fact]
        public async Task Should_update_location_with_new_path()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_SkipTo_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_SkipTo_Scroll_Focus_Func, _ => true);

            var skipTo = context.Render<SkipToComponent>(paramBuilder => paramBuilder.Add(p => p.TargetID, "main-content"));

            var target = skipTo.Find("a").GetAttribute("href");

            var navManager = context.Services.GetRequiredService<NavigationManager>();

            navManager.NavigateTo("/new-page");

            skipTo.WaitForAssertion(() =>
            {
                var updatedHref = skipTo.Find("a").GetAttribute("href");
                updatedHref.Should().Be("new-page#main-content");
            });
        }
    }
}
