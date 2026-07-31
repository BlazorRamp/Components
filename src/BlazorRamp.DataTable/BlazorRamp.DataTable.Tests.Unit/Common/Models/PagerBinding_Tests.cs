using BlazorRamp.DataTable.Common.Models;
using FluentAssertions;

namespace BlazorRamp.DataTable.Tests.Unit.Common.Models;

public class PagerBinding_Tests
{
    [Fact]
    public void Should_be_able_to_create_a_pager_binder_with_the_provide_constructor_values()
    {
        var pagerBinding = new PagerBinding(currentPage: 10, currentItemCount: 1000, totalItemCount: 2000, itemsPerPage: 50);

        pagerBinding.Should().Match<PagerBinding>(b => b.CurrentPage == 10 && b.CurrentItemCount == 1000 && b.TotalItemCount == 2000 && b.ItemsPerPage == 50);
    }
}
