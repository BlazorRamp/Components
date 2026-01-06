using BlazorRamp.Core.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BlazorRamp.Core.Tests.Unit.Components.LiveRegion;

public class LiveRegion_Tests
{
    [Fact]
    public void Should_render_both_live_region_elements()
    {

        using var context = new BunitContext(); ;

        var liveRegionComponent = context.Render<BlazorRamp.Core.Components.LiveRegion.LiveRegion>();

        var liveRegions = liveRegionComponent.Find($"#{@CoreGlobalValues.Live_Regions_ID}");

        // Assert
        var assertiveRegionOne = liveRegionComponent.Find($"#{CoreGlobalValues.Live_Region_Assertive_One_ID}");
        var assertiveRegionTwo = liveRegionComponent.Find($"#{CoreGlobalValues.Live_Region_Assertive_Two_ID}");
        var politeRegionOne    = liveRegionComponent.Find($"#{CoreGlobalValues.Live_Region_Polite_One_ID}");
        var politeRegionTwo   = liveRegionComponent.Find($"#{CoreGlobalValues.Live_Region_Polite_Two_ID}");

        using (new AssertionScope())
        {

            liveRegions.ClassList.Should().Contain(CoreGlobalValues.CSS_Visually_Hidden_Class);
            assertiveRegionOne.GetAttribute("aria-live").Should().Be("assertive");
            assertiveRegionTwo.GetAttribute("aria-live").Should().Be("assertive");
            politeRegionOne.GetAttribute("aria-live").Should().Be("polite");
            politeRegionTwo.GetAttribute("aria-live").Should().Be("polite");
        }

    }
}
