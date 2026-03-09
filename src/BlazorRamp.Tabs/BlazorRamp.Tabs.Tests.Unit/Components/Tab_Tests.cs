using BlazorRamp.Tabs.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata.Ecma335;
using TabComponent = BlazorRamp.Tabs.Components.Tab;
using TabsComponent = BlazorRamp.Tabs.Components.Tabs;

namespace BlazorRamp.Tabs.Tests.Unit.Components;

public class Tab_Tests
{        
    public static (IRenderedComponent<TabsComponent> tabs, IRenderedComponent<TabComponent> tab) CreateTabWithTabParamByName<TValue>(BunitContext context, string tabParamName, TValue tabParamValue, string tabTitle = "Tab One", int tabIndex = 0)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
        moduleInterop.SetupVoid("registerTabs", _ => true).SetVoidResult();

        var tabsComponent =  context.Render<TabsComponent>(paramBuilder =>
        {
            paramBuilder.Add(p => p.AriaLabel, "Tabs");
            paramBuilder.AddChildContent<TabComponent>(compBuilder =>
            {
                compBuilder.Add(p => p.TabTitle, "Tab One");
                compBuilder.Add(p => p.TabPanelContent, "<p>Panel One Content</p>");
                compBuilder.TryAdd<TValue>(tabParamName, tabParamValue);
            });
            paramBuilder.AddChildContent<TabComponent>(compBuilder => compBuilder.Add(p => p.TabTitle, "Tab Two"));
        });

        return (tabsComponent, tabsComponent.FindComponents<TabComponent>()[tabIndex]);
    }
    public class Parameters
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("Tab1")]
        public void Should_be_able_to_set_the_tab_title_with_an_exception_raised_if_null_empty_or_whitespace(string? tabTitle)
        {
            var context = new BunitContext();
            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid("registerTabs", _ => true).SetVoidResult();

            if (String.IsNullOrWhiteSpace(tabTitle))
            {
                FluentActions.Invoking(() =>
                {
                    context.Render<TabsComponent>(paramBuilder =>
                    {
                        paramBuilder.Add(p => p.AriaLabel, "Tabs");
                        paramBuilder.AddChildContent<TabComponent>(compBuilder =>
                        {
                            compBuilder.Add(p => p.TabTitle, tabTitle);
                        });
                    });
                }).Should().Throw<ArgumentNullException>();

                return;
            }

            context.Render<TabsComponent>(paramBuilder =>
            {
                paramBuilder.Add(p => p.AriaLabel, "Tabs");
                paramBuilder.AddChildContent<TabComponent>(compBuilder =>
                {
                    compBuilder.Add(p => p.TabTitle, tabTitle);
                });
            }).Find($".{GlobalValues.Tabs_Tab_Title_Class}").TextContent.Should().Be(tabTitle);

        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_persist_content_param_which_should_persist_when_the_tab_is_not_active(bool persistContent)
        {
            var context = new BunitContext();
            var (tabs, tab) = CreateTabWithTabParamByName<bool>(context, nameof(TabComponent.PersistContent), persistContent);
           

            using (new AssertionScope())
            {
                tab.Instance.PersistContent.Should().Be(persistContent);
                await tabs.Instance.SetActiveTab(1);
                
                if(true == persistContent)
                {
                    tab.Find("[role='tabpanel']").InnerHtml.Should().NotBeEmpty();
                }
                else
                {
                    tab.Find("[role='tabpanel']").InnerHtml.Should().BeEmpty();
                }
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_be_able_to_set_the_has_tabindex_param_to_put_a_tab_index_on_the_tab_panel(bool hasPanelTabIndex)
        {
            var context = new BunitContext();
            var (tabs, tab) = CreateTabWithTabParamByName<bool>(context, nameof(TabComponent.HasPanelTabIndex),hasPanelTabIndex);

            using (new AssertionScope()) 
            { 
                tab.Instance.HasPanelTabIndex.Should().Be(hasPanelTabIndex);

                if(true ==  hasPanelTabIndex)
                {
                    tab.Find("[role='tabpanel']").GetAttribute("tabindex").Should().Be("0");
                }
                else
                {
                    tab.Find("[role='tabpanel']").GetAttribute("tabindex").Should().BeNull();
                }
            }
        }

        [Theory]
        [InlineData("--svg-icon")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]

        public void Should_be_able_to_set_the_optional_svg_icon_paramater_which_must_start_with_a_double_dash(string svgIconVariable)
        {
            var context = new BunitContext();
            var (tabs, tab) = CreateTabWithTabParamByName<string?>(context, nameof(TabComponent.SvgIcon), svgIconVariable);


            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                tabs.FindAll($".{GlobalValues.Tabs_Tab_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                tabs.FindAll($".{GlobalValues.Tabs_Tab_Icon_Class}")[0].GetAttribute("style").Should().NotBeEmpty();
            }
            else
            {
                tabs.FindAll($".{GlobalValues.Tabs_Tab_Icon_Class}").Should().BeEmpty();
            }

        }

    }

    public class OnIntialised()
    {
        [Fact]
        public void Should_throw_when_tab_is_used_outside_of_tabs_component()
        {
            var context = new BunitContext();

            FluentActions.Invoking(() => context.Render<TabComponent>(pb => pb.Add(p => p.TabTitle, "Tab One"))).Should().Throw<ArgumentNullException>();
        }
    }

    public class Dispose()
    {
        [Fact]
        public void Should_remove_tab_from_parent_when_disposed()
        {
            var context = new BunitContext();
            
            var (tabs, tab) = Tab_Tests.CreateTabWithTabParamByName<bool>(context, nameof(TabComponent.PersistContent),true);

            tabs.FindAll("button").Count.Should().Be(2);

            tab.Instance.Dispose();
            tabs.Render();

            tabs.FindAll("button").Count.Should().Be(1);
        }
    }
}
