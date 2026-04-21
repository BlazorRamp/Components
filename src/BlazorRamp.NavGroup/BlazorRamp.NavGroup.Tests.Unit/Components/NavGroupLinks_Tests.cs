
using BlazorRamp.NavGroup.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NavGroupLinkComponent = BlazorRamp.NavGroup.Components.NavGroupLink;

namespace BlazorRamp.NavGroup.Tests.Unit.Components;

public class NavGroupLinks_Tests
{
    private static IRenderedComponent<NavGroupLinkComponent> CreateNavGroupLinkWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, string linkText = "My Link")

        => context.Render<NavGroupLinkComponent>(paramBuilder => paramBuilder.Add(p => p.LinkText, linkText).TryAdd(paramName, paramValue));



    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_throw_if_link_text_is_null_empty_or_whitespace(string? linkText)
    {
        using var context = new BunitContext();

        FluentActions.Invoking(() =>  context.Render<NavGroupLinkComponent>(paramBuilder => paramBuilder.Add(p => p.LinkText, linkText!))).Should().ThrowExactly<ArgumentNullException>();
    }

    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("someurl")]
        public void Should_default_to_missing_href_if_href_is_null_empty_or_whitespace(string? href)
        {
            using var context = new BunitContext();

            var navLink = CreateNavGroupLinkWithParamByName<string?>(context, nameof(NavGroupLinkComponent.Href), href);

            var hrefValue = navLink.Find("a").GetAttribute("href");

            if (String.IsNullOrWhiteSpace(href)) 
            {
                hrefValue.Should().Be(GlobalValues.Missing_Href_Value);
                return;
            }

            hrefValue.Should().Be(href);
        }

        [Theory]
        [InlineData("My Link")]
        [InlineData("My Link   ")]
        [InlineData("   My Link   ")]
        [InlineData("   My Link")]
        public void Should_trim_leading_and_trailing_spaces_from_link_text(string linkText)
        {
            using var context = new BunitContext();

            var navLink = context.Render<NavGroupLinkComponent>(paramBuilder => paramBuilder.Add(p => p.LinkText, linkText!));

            navLink.Find("a").TextContent.Should().Be(linkText.Trim());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("my-prefix")]

        public void Should_render_visually_hidden_prefix_when_set(string? prefix)
        {
            using var context = new BunitContext();
            var navLink = CreateNavGroupLinkWithParamByName<string?>(context, nameof(NavGroupLinkComponent.VisuallyHiddenPrefix), prefix);

            var spans = navLink.FindAll(".br-visually-hidden");


            if (String.IsNullOrWhiteSpace(prefix))
            {
                spans.Should().BeEmpty();
                return;
            }

            spans[0].InnerHtml.Should().Be(prefix + "&nbsp;");//&nbsp; is added
        }

        [Fact]
        public void Should_have_aria_current_page_when_href_matches_current_url()
        {
            using var context = new BunitContext();

            var navManager = context.Services.GetRequiredService<NavigationManager>();

            navManager.NavigateTo("/test-url");

            var navLink = CreateNavGroupLinkWithParamByName<string>(context, nameof(NavGroupLinkComponent.Href), "/test-url");

            navLink.Find($".{GlobalValues.Nav_Group_Link_Class}").GetAttribute("aria-current").Should().Be("page");
        }
        [Fact]
        public void Should_set_aria_current_page_on_navigation_to_matching_url()
        {
            using var context = new BunitContext();

            var navManager = context.Services.GetRequiredService<NavigationManager>();

            var navLink = CreateNavGroupLinkWithParamByName<string>(context, nameof(NavGroupLinkComponent.Href), "/test-url");

            navLink.Find($".{GlobalValues.Nav_Group_Link_Class}").GetAttribute("aria-current").Should().BeNull();

            navManager.NavigateTo("/test-url");

            navLink.Find($".{GlobalValues.Nav_Group_Link_Class}").GetAttribute("aria-current").Should().Be("page");
        }

        [Fact]
        public void Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            using var context = new BunitContext();

            var navLinkComponent = context.Render<NavGroupLinkComponent>(parameters => parameters.Add(p => p.LinkText, "My Link Text").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                navLinkComponent.Find("a").GetAttribute("style").Should().Be("color:red;");
                navLinkComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

            }
        }


        [Theory]
        [InlineData(TargetType.Self)]
        [InlineData(TargetType.Blank)]
        [InlineData(TargetType.Parent)]
        [InlineData(TargetType.Top)]
        [InlineData((TargetType)99)]
        public void Should_be_able_to_set_the_target_type_with_the_default_of_self(TargetType targetType)
        {
            using var context = new BunitContext();

            var navLink = CreateNavGroupLinkWithParamByName<TargetType>(context, nameof(NavGroupLinkComponent.TargetType), targetType);

            var targetTypeAttribute = navLink.Find("a").GetAttribute("target");

            if ((int)targetType == 99)
            {
                targetTypeAttribute.Should().Be($"_{TargetType.Self.ToString().ToLower()}");
                return;
            }

            targetTypeAttribute.Should().Be($"_{Enum.GetName<TargetType>(targetType)!.ToLower()}");
        }



        [Theory]
        [InlineData("--svg-icon")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]

        public async Task Should_be_able_to_set_the_optional_svg_icon_paramater_which_must_start_with_a_double_dash(string svgIconVariable)
        {
            await using var context = new BunitContext();
            
            var navLink = CreateNavGroupLinkWithParamByName<string?>(context, nameof(NavGroupLinkComponent.SvgIcon), svgIconVariable);

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                navLink.FindAll($"a > span.{GlobalValues.Nav_Group_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                navLink.FindAll($"a > span.{GlobalValues.Nav_Group_Icon_Class}")[0].GetAttribute("style").Should().NotBeEmpty();
            }
            else
            {
                navLink.FindAll($"a > span.{GlobalValues.Nav_Group_Icon_Class}").Should().BeEmpty();
            }

        }

    }
}
