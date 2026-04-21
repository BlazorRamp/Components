
using BlazorRamp.NavGroup.Common.Constants;
using BlazorRamp.NavGroup.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NavSectionComponent = BlazorRamp.NavGroup.Components.NavSection;

namespace BlazorRamp.NavGroup.Tests.Unit.Components;

public class NavSection_Tests
{
    private static IRenderedComponent<NavSectionComponent> CreateNavSectionWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, string title = "My Title")

        => context.Render<NavSectionComponent>(paramBuilder => paramBuilder.Add(p => p.Title, title).TryAdd(paramName, paramValue));

    public class Parameters
    {
        [Fact]
        public void Should_be_able_to_render_child_component()
        {
            using var context = new BunitContext();

            var navSectionComponent = context.Render<NavSectionComponent>(paramBuilder =>
            {
                paramBuilder.Add(p => p.Title, "Nav Section").Add(p => p.Expanded, false);
                paramBuilder.AddChildContent<NavGroupLink>(innerBuilder =>
                {
                    innerBuilder.Add(p => p.LinkText, "Link Text");
                });
            });

            navSectionComponent.Find("a").Should().NotBeNull();
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_set_the_starting_state_of_the_section_to_either_expanded_or_collapsed(bool expanded)
        {
            using var context = new BunitContext();

            var navSection = CreateNavSectionWithParamByName<bool>(context, nameof(NavSectionComponent.Expanded), expanded);


            var expanedAttribute = navSection.Find("button").GetAttribute("aria-expanded");
            
            expanedAttribute.Should().Be(expanded.ToString().ToLower());
            navSection.Instance.Expanded.Should().Be(expanded);

        }

        [Theory]
        [InlineData("--svg-icon")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]

        public async Task Should_be_able_to_set_the_optional_svg_icon_paramater_which_must_start_with_a_double_dash(string svgIconVariable)
        {
            await using var context = new BunitContext();

            var navSection = CreateNavSectionWithParamByName<string?>(context, nameof(NavSectionComponent.SvgIcon), svgIconVariable);

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                navSection.FindAll($"button > span.{GlobalValues.Nav_Group_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                navSection.FindAll($"button > span.{GlobalValues.Nav_Group_Icon_Class}")[0].GetAttribute("style").Should().NotBeEmpty();
            }
            else
            {
                navSection.FindAll($"button > span.{GlobalValues.Nav_Group_Icon_Class}").Should().BeEmpty();
            }

        }


    }

    public class SetExpandedState
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_expand_or_collapse_the_section_programatically(bool isExpanded)
        {
            await using var context = new BunitContext();

            var navSection = CreateNavSectionWithParamByName<bool>(context, nameof(NavSectionComponent.Expanded), !isExpanded);//set initial state to the opposite

            var startState = navSection.Find("button").GetAttribute("aria-expanded");

            await navSection.InvokeAsync(() => navSection.Instance.SetExpandedState(isExpanded));

            var endState = navSection.Find("button").GetAttribute("aria-expanded");

           using(new AssertionScope())
            {
                startState.Should().Be((!isExpanded).ToString().ToLower());
                endState.Should().Be((isExpanded).ToString().ToLower());
            }
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Clicking_the_nav_section_button_should_expand_or_collapse_the_section(bool isExpanded)
        {
            await using var context = new BunitContext();

            var navSection = CreateNavSectionWithParamByName<bool>(context, nameof(NavSectionComponent.Expanded), !isExpanded);//set initial state to the opposite

            var startState = navSection.Find("button").GetAttribute("aria-expanded");

            await navSection.Find("button").ClickAsync();

            var endState = navSection.Find("button").GetAttribute("aria-expanded");

            using (new AssertionScope())
            {
                startState.Should().Be((!isExpanded).ToString().ToLower());
                endState.Should().Be((isExpanded).ToString().ToLower());
            }
        }
    }

    public class Internal_ExpandParent
    {
        [Fact]
        public async Task Should_expand_parent_section_when_called()
        {
            await using var context = new BunitContext();

            var parentSection = context.Render<NavSectionComponent>(paramBuilder =>
            {
                paramBuilder.Add(p => p.Title, "Parent Section").Add(p => p.Expanded, false);
                paramBuilder.AddChildContent<NavSectionComponent>(innerBuilder =>
                {
                    innerBuilder.Add(p => p.Title, "Child Section");
                });
            });

            var childSection = parentSection.FindComponent<NavSectionComponent>();


            var parentStartState = parentSection.Find("button").GetAttribute("aria-expanded");

            await childSection.InvokeAsync(() => childSection.Instance.ExpandParent());

            var parentEndState = parentSection.Find("button").GetAttribute("aria-expanded");

            using( new AssertionScope())
            {
                parentStartState.Should().Be("false");
                parentEndState.Should().Be("true");
            }
        }
    }

    public class Navigation_LocationChanged
    {
        [Fact]
        public async Task Should_iterate_nav_links_and_on_a_current_page_match_should_expand_if_collapsed()
        {

            using var context = new BunitContext();

            var navManager = context.Services.GetRequiredService<NavigationManager>();

            var navSectionComponent = context.Render<NavSectionComponent>(paramBuilder =>
            {
                paramBuilder.Add(p => p.Title, "Nav Section").Add(p => p.Expanded, false);
                paramBuilder.AddChildContent<NavGroupLink>(innerBuilder =>
                {
                    innerBuilder.Add(p => p.LinkText, "Link Text").Add(p => p.Href,"my-url");
                });
            });

            var starPageState = navSectionComponent.FindComponent<NavGroupLink>().Find("a").GetAttribute("aria-current");

            navManager.NavigateTo("my-url");

            var endPageState = navSectionComponent.FindComponent<NavGroupLink>().Find("a").GetAttribute("aria-current");

            using(new AssertionScope())
            {
                starPageState.Should().BeNull();
                endPageState.Should().Be("page");
            }
        }
    }
}
