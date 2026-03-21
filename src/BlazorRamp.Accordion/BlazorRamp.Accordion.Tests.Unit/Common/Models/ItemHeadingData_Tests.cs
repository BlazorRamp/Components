using BlazorRamp.Accordion.Common.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Accordion.Tests.Unit.Common.Models;

public class ItemHeadingData_Tests
{
    [Fact]
    public void Should_Be_able_to_set_all_the_properties_via_the_primary_constructor()
    {
        var itemIndex      = 3;
        var headtingTest   = "Heading";
        var isExpanded     = true;
        var itemHadingData = new ItemHeadingData(itemIndex, headtingTest, isExpanded);

        itemHadingData.Should().Match<ItemHeadingData>(r => r.ItemIndex == itemIndex & r.IsExpanded == isExpanded && r.HeadingText == headtingTest);
    }

    [Fact]
    public void Should_be_able_to_create_using_with()
    {
        var itemIndex = 3;
        var headtingTest = "Heading";
        var isExpanded = true;
        var itemHadingData = new ItemHeadingData(itemIndex, headtingTest, isExpanded);

        var clone = itemHadingData with { ItemIndex= itemIndex + 1, HeadingText = headtingTest + "1" , IsExpanded = false};

        clone.Should().Match<ItemHeadingData>(r => r.ItemIndex == itemIndex + 1 & r.IsExpanded == false && r.HeadingText == headtingTest + 1);
    }
}
