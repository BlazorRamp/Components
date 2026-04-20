
using BlazorRamp.NavGroup.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using NavGroupComponent = BlazorRamp.NavGroup.Components.NavGroup;

namespace BlazorRamp.NavGroup.Tests.Unit.Components;

public class NavGroup_Tests
{
    private static IRenderedComponent<NavGroupComponent> CreateNavGroupWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)

        => context.Render<NavGroupComponent>(paramBuilder => paramBuilder.TryAdd(paramName, paramValue));


    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("SomeID")]
        public void Should_be_able_to_set_the_aria_labelled_by_param_if_not_null_empty_or_whitespace(string? ariaLabelledby)
        {
            using var context = new BunitContext();

            var navGroupComponent = CreateNavGroupWithParamByName<string?>(context, nameof(NavGroupComponent.AriaLabelledBy), ariaLabelledby);

            if (String.IsNullOrWhiteSpace(ariaLabelledby))
            {
                navGroupComponent.Find("ul").GetAttribute("aria-labelledby").Should().BeNull();
                return;
            }

            navGroupComponent.Find("ul").GetAttribute("aria-labelledby").Should().Be(ariaLabelledby);
        }

        [Fact]
        public void Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            using var context = new BunitContext();

            var navGroupComponent = context.Render<NavGroupComponent>(parameters => parameters.AddUnmatched("style", "color:red;"));

            using(new AssertionScope())
            {
                navGroupComponent.Find("ul").GetAttribute("style").Should().Be("color:red;");
                navGroupComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

            }

        }
        [Fact]
        public void Should_be_able_to_render_child_component()
        {
            using var context = new BunitContext();

            var navGroupComponent = context.Render<NavGroupComponent>(parameters => parameters.AddChildContent<NavSeparator>());

            navGroupComponent.Find("li").Should().NotBeNull();
        }
    }
}
