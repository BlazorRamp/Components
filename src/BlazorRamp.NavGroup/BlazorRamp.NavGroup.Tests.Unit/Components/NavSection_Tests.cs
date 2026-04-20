
using BlazorRamp.NavGroup.Components;
using Bunit;
using FluentAssertions;
using NavSectionComponent = BlazorRamp.NavGroup.Components.NavSection;

namespace BlazorRamp.NavGroup.Tests.Unit.Components;

public class NavSection_Tests
{
    private static IRenderedComponent<NavSectionComponent> CreateNavGroupLinkWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, string title = "My Title")

        => context.Render<NavSectionComponent>(paramBuilder => paramBuilder.Add(p => p.Title, title).TryAdd(paramName, paramValue));

    public class Parameters
    {
        [Fact]
        public void Should_be_able_to_render_child_component()
        {
            using var context = new BunitContext();

            var navGroupComponent = context.Render<NavSectionComponent>(parameters => parameters.Add(p => p.Title, "My Title").AddChildContent<NavSeparator>());

            navGroupComponent.Find("li").Should().NotBeNull();
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_set_the_starting_state_of_the_section_to_either_expanded_or_collapsed(bool expanded)
        {
            using var context = new BunitContext();

            var navSection = CreateNavGroupLinkWithParamByName<bool>(context, nameof(NavSectionComponent.Expanded), expanded);


            var expanedAttribute = navSection.Find("button").GetAttribute("aria-expanded");
            
            expanedAttribute.Should().Be(expanded.ToString().ToLower());
            navSection.Instance.Expanded.Should().Be(expanded);


        }




    }

}
