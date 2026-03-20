

using BlazorRamp.Tabs.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Web;
using TabComponent = BlazorRamp.Tabs.Components.Tab;
using TabsComponent = BlazorRamp.Tabs.Components.Tabs;

namespace BlazorRamp.Tabs.Tests.Unit.Components;

public class Tabs_Tests
{
    public static IRenderedComponent<TabsComponent> CreateTabsWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
        moduleInterop.SetupVoid("registerTabs", _ => true).SetVoidResult();

        return context.Render<TabsComponent>(paramBuilder =>
        {
            paramBuilder.TryAdd<TValue>(paramName, paramValue);

            paramBuilder.AddChildContent<TabComponent>(compBuilder => compBuilder.Add(p => p.TabTitle, "Tab One"));
            paramBuilder.AddChildContent<TabComponent>(compBuilder => compBuilder.Add(p => p.TabTitle, "Tab Two"));
        });

    }
    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("SomeID")]
        public async Task Should_be_able_to_set_the_aria_labelled_by_param_if_not_null_empty_or_whitespace(string? ariaLabelledby)
        {
            await using var context = new BunitContext();

            var tabsComponent = CreateTabsWithParamByName<string?>(context, nameof(TabsComponent.AriaLabelledBy), ariaLabelledby);

            if (String.IsNullOrWhiteSpace(ariaLabelledby))
            {
                tabsComponent.Find("div > div").GetAttribute("aria-labelledby").Should().BeNull();
                return;
            }

            tabsComponent.Find("div > div").GetAttribute("aria-labelledby").Should().Be(ariaLabelledby);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("SomeID")]
        public async Task Should_be_able_to_set_the_aria_label_param_with_a_default_used_if_null_empty_or_whitespace(string? ariaLabel)
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AriaLabel), ariaLabel);

            if (String.IsNullOrWhiteSpace(ariaLabel))
            {
                tabsComponent.Find("div > div").GetAttribute("aria-label").Should().Be(GlobalValues.Tabs_Default_ACC_Name);
                return;
            }
            tabsComponent.Find("div > div").GetAttribute("aria-label").Should().Be(ariaLabel);
        }

        [Fact]
        public async Task Should_be_able_to_set_the_active_tab_index_param_which_defaults_to_zero()
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.ActiveTabIndex), 1);

            var tabList = tabsComponent.WaitForElement("[role='tablist']");
            var buttons = tabList.QuerySelectorAll("button");

            using (new AssertionScope())
            {
                buttons[0].GetAttribute("aria-selected").Should().Be("false");
                buttons[1].GetAttribute("aria-selected").Should().Be("true");
            }

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_auto_activate_paramater(bool autoActivate)
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AutoActivatePanel), autoActivate);

            tabsComponent.Instance.AutoActivatePanel.Should().Be(autoActivate);

        }

        [Fact]
        public async Task Should_be_able_to_set_the_active_index_programatically()
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AriaLabel), "Create Component");

            await tabsComponent.Instance.SetActiveTab(1);

            var buttons = tabsComponent.FindAll("button");
            buttons[1].GetAttribute("aria-selected").Should().Be("true");

        }

        [Theory]
        [InlineData(TabIconPosition.Top)]
        [InlineData(TabIconPosition.Bottom)]
        [InlineData(TabIconPosition.Left)]
        [InlineData(TabIconPosition.Right)]

        public async Task Should_be_able_to_set_the_tab_icon_position_used_for_all_tabs(TabIconPosition iconPosition)
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.TabIconPosition), iconPosition);

            switch (iconPosition)
            {
                case TabIconPosition.Top:
                    tabsComponent.FindAll($".{GlobalValues.Tabs_Tab_Content_Icon_Top_Mdoifier}").Should().NotBeEmpty();
                    break;
                case TabIconPosition.Bottom:
                    tabsComponent.FindAll($".{GlobalValues.Tabs_Tab_Content_Icon_Bottom_Mdoifier}").Should().NotBeEmpty();
                    break;
                case TabIconPosition.Left:
                    tabsComponent.FindAll($".{GlobalValues.Tabs_Tab_Content_Icon_Left_Mdoifier}").Should().NotBeEmpty();
                    break;
                case TabIconPosition.Right:
                    tabsComponent.FindAll($".{GlobalValues.Tabs_Tab_Content_Icon_Right_Mdoifier}").Should().NotBeEmpty();
                    break;

            }
        }

        [Fact]
        public async Task Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid("registerTabs", _ => true).SetVoidResult();

            var tabsComponent = context.Render<TabsComponent>(parameters => parameters.AddUnmatched("style", "color:red;"));

            tabsComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");
        }

        [Fact]
        public async Task Should_change_active_tab_when_active_tab_index_param_changes()
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.ActiveTabIndex), 0);

            tabsComponent.WaitForElement("[role='tablist']");

            tabsComponent.Render(pb => pb.TryAdd(nameof(TabsComponent.ActiveTabIndex), 1));

            var tabList = tabsComponent.WaitForElement("[role='tablist']");
            var buttons = tabList.QuerySelectorAll("button");

            buttons[0].GetAttribute("aria-selected").Should().Be("false");
            buttons[1].GetAttribute("aria-selected").Should().Be("true");
        }

        [Fact]
        public async Task Should_remove_tab_when_remove_tab_is_called()
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AriaLabel), "Tabs");

            tabsComponent.WaitForElement("[role='tablist']");
            tabsComponent.FindAll("button").Count.Should().Be(2);

            var tab = tabsComponent.FindComponent<TabComponent>().Instance;
            tabsComponent.Instance.RemoveTab(tab);
            tabsComponent.Render();

            tabsComponent.FindAll("button").Count.Should().Be(1);
        }




        [Fact]
        public async Task Should_auto_activate_tab_on_focus_when_auto_activate_panel_is_true()
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AutoActivatePanel), true);

            tabsComponent.WaitForElement("[role='tablist']");

            tabsComponent.FindAll("button")[1].Focus();

            tabsComponent.FindAll("button")[1].GetAttribute("aria-selected").Should().Be("true");
        }
        [Fact]
        public async Task Should_activate_tab_when_clicked()
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AriaLabel), "Tabs");

            tabsComponent.WaitForElement("[role='tablist']");

            tabsComponent.FindAll("button")[1].Click();

            tabsComponent.FindAll("button")[1].GetAttribute("aria-selected").Should().Be("true");
            tabsComponent.FindAll("button")[0].GetAttribute("aria-selected").Should().Be("false");
        }


        [Theory]
        //[InlineData("ArrowRight", 1)]
        [InlineData("Home", 0)]
        //[InlineData("End", 1)]
        public async Task Should_move_roving_tabindex_on_keyboard_navigation(string key, int expectedIndex)
        {
            await using var context = new BunitContext();
            var tabsComponent = CreateTabsWithParamByName(context, nameof(TabsComponent.AriaLabel), "Tabs");

            tabsComponent.WaitForElement("[role='tablist']");

            tabsComponent.Find("[role='tablist']").KeyDown(new KeyboardEventArgs { Key = key });

            tabsComponent.WaitForAssertion(() =>
            {
                var buttons = tabsComponent.FindAll("button");
                buttons[expectedIndex].GetAttribute("tabindex").Should().Be("0");
            });
        }
    }
}