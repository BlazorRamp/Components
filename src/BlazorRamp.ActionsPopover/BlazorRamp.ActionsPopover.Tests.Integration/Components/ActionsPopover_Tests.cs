using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;
using Xunit;

namespace BlazorRamp.ActionsPopover.Tests.Integration;

public class ActionsPopover_Tests : PageTest
{
    private const string BaseUrl = "https://localhost:44348";

    [Fact]
    public async Task Should_open_the_popover_and_show_all_action_items_when_trigger_is_clicked()
    {
        await Page.GotoAsync(BaseUrl);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Actions" }).ClickAsync();

        var popover = Page.GetByLabel("Actions");

        await Expect(popover.GetByText("My Action One")).ToBeVisibleAsync();
        await Expect(popover.GetByText("My Action Two")).ToBeVisibleAsync();
        await Expect(popover.GetByText("Weather", new() { Exact = true })).ToBeVisibleAsync();
        await Expect(popover.GetByText("Weather Prevent Default")).ToBeVisibleAsync();
    }
}