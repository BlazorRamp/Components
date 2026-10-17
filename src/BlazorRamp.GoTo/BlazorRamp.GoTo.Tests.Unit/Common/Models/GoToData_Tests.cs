using BlazorRamp.GoTo.Common.Models;
using FluentAssertions;

namespace BlazorRamp.GoTo.Tests.Unit.Common.Models;

public class GoToData_Tests
{

    [Fact]
    public void Should_be_able_to_construct_a_new_go_to_data_model()
    {
        var gotoData = new GoToData("MarkerID", "PopoverID", "Text", "Number");

        gotoData.Should().Match<GoToData>(m => m.MarkerID == "MarkerID" && m.PopoverID == "PopoverID" 
                                          && m.GoToText == "Text" &&  m.GoToNumber == "Number");
    }
}
