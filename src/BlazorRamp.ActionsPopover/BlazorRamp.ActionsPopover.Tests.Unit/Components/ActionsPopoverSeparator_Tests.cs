using BlazorRamp.ActionsPopover.Components;
using Bunit;
using FluentAssertions;

namespace BlazorRamp.ActionsPopover.Tests.Unit.Components;

public class ActionsPopoverSeparator_Tests
{
    public static IRenderedComponent<ActionPopoverSeparator> CreateActionPopoverSeparator(
       BunitContext context,
       Action<ComponentParameterCollectionBuilder<ActionPopoverSeparator>>? parameters = null)
    {
        var component = context.Render<ActionPopoverSeparator>(
            builder => parameters?.Invoke(builder));

        return component;
    }
    public class Parameters
    {
        [Fact]
        public async Task Should_render_a_hr_element()
        {
            await using var context = new BunitContext();

            var component = CreateActionPopoverSeparator(context);

            component.Find("hr").Should().NotBeNull();

        }

    }
}
