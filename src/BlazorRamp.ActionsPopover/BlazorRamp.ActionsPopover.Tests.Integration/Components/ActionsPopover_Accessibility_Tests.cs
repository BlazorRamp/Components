using BlazorRamp.ActionsPopover.Tests.Integration.Fixtures;
using Deque.AxeCore.Playwright;
using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace BlazorRamp.ActionsPopover.Tests.Integration;


[Collection("Sandbox")]
public class ActionsPopover_Accessibility_Tests(SandboxServerFixture sandbox) : PageTest
{

    private async Task<ILocator> OpenPopoverAsync()
    {
        await Page.GotoAsync(sandbox.BaseUrl);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Actions" }).ClickAsync();

        return Page.Locator(".br-actions-popover");
    }

    [Fact]
    [Trait("Category", "Accessibility")]
    public async Task Should_have_no_axe_violations_when_the_popover_is_open()
    {
        var popoverContainer = await OpenPopoverAsync();

        var results = await popoverContainer.RunAxe();

        results.Violations.Should().BeEmpty();
    }

    [Fact]
    [Trait("Category", "Accessibility")]
    public async Task Should_close_the_popover_when_escape_is_pressed()
    {
        var popoverContainer = await OpenPopoverAsync();
        var panel = popoverContainer.Locator(".br-actions-popover__panel");

        await Page.Keyboard.PressAsync("Escape");

        await Expect(panel).ToBeHiddenAsync();
    }
    [Fact]
    [Trait("Category", "Accessibility")]
    public async Task Should_close_the_popover_when_focus_tabs_past_the_last_item()
    {
        var popoverContainer = await OpenPopoverAsync();
        var panel = popoverContainer.Locator(".br-actions-popover__panel");

        for (var i = 0; i < 5; i++)
        {
            await Page.Keyboard.PressAsync("Tab");
        }

        await Expect(panel).ToBeHiddenAsync();
    }
}