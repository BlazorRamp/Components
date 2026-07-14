using BlazorRamp.ActionsPopover.Tests.Integration.Fixtures;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;
using System.Text.RegularExpressions;
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


    [Theory]
    [InlineData("/", 5_000)]
    [InlineData("/counter", 10_000)]
    public async Task Should_navigate_to_the_target_page_when_prevent_default_is_false(string route, int triggerTimeoutMs)
    {
        await Page.GotoAsync($"{sandbox.BaseUrl}{route}");

        var trigger = Page.GetByRole(AriaRole.Button, new() { Name = "Actions" });
        await Expect(trigger).ToBeVisibleAsync(new() { Timeout = triggerTimeoutMs });
        await trigger.ClickAsync();

        var popover = Page.GetByLabel("Actions");
        await popover.GetByText("Weather", new() { Exact = true }).ClickAsync();

        await Expect(Page).ToHaveURLAsync($"{sandbox.BaseUrl}/weather");
    }

    [Theory]
    [InlineData("/", 5_000)]
    [InlineData("/counter", 10_000)]
    public async Task Should_not_navigate_when_prevent_default_is_true(string route, int triggerTimeoutMs)
    {
        var startUrl = $"{sandbox.BaseUrl}{route}";
        await Page.GotoAsync(startUrl);

        var trigger = Page.GetByRole(AriaRole.Button, new() { Name = "Actions" });
        await Expect(trigger).ToBeVisibleAsync(new() { Timeout = triggerTimeoutMs });
        await trigger.ClickAsync();

        var popover = Page.GetByLabel("Actions");
        await popover.GetByText("Weather Prevent Default").ClickAsync();

        await Page.WaitForTimeoutAsync(300); // give an accidental navigation a moment to occur before asserting it didn't

        await Expect(Page).ToHaveURLAsync(new Regex($"^{Regex.Escape(startUrl.TrimEnd('/'))}/?$"));
    }
}