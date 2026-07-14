using BlazorRamp.ActionsPopover.Tests.Integration.Fixtures;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;
using Xunit;

namespace BlazorRamp.ActionsPopover.Tests.Integration;


[Collection("Sandbox")]
public class ActionsPopover_Tests(SandboxServerFixture sandbox) : PageTest
{
    [Theory]
    [InlineData("/", 5_000)]          // InteractiveServer — near-instant over the SignalR circuit
    [InlineData("/counter", 10_000)]  // InteractiveWebAssembly — first hit downloads/boots the runtime
    public async Task Should_open_the_popover_and_show_all_action_items_when_trigger_is_clicked(
        string route, int triggerTimeoutMs)
    {
        await Page.GotoAsync($"{sandbox.BaseUrl}{route}");

        var trigger = Page.GetByRole(AriaRole.Button, new() { Name = "Actions" });
        await Expect(trigger).ToBeVisibleAsync(new() { Timeout = triggerTimeoutMs });

        await trigger.ClickAsync();

        var popover = Page.GetByLabel("Actions");

        await Expect(popover.GetByText("My Action One")).ToBeVisibleAsync();
        await Expect(popover.GetByText("My Action Two")).ToBeVisibleAsync();
        await Expect(popover.GetByText("Weather", new() { Exact = true })).ToBeVisibleAsync();
        await Expect(popover.GetByText("Weather Prevent Default")).ToBeVisibleAsync();
    }
}