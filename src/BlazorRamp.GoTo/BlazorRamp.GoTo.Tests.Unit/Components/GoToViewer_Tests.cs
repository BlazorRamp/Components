using BlazorRamp.GoTo.Common.Constants;
using BlazorRamp.GoTo.Common.Models;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using GoToViewerComponent = BlazorRamp.GoTo.Components.GoToViewer;
namespace BlazorRamp.GoTo.Tests.Unit.Components;

public  class GoToViewer_Tests
{
    public static readonly GoToData[] _markers =
    [
        new("marker-one", "label-one", "First marker",  "01"),
        new("marker-two", "label-two", "Second marker", "02")
    ];

    public static (IRenderedComponent<GoToViewerComponent> Component, BunitJSModuleInterop ModuleInterop) CreateGoToViewer(
        BunitContext context,
        Action<ComponentParameterCollectionBuilder<GoToViewerComponent>>? parameters = null,
        GoToData[]? markersOnPage = null)
    {
        // The viewer imports its own module on first render, so fake the module and every function it calls.
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);

        moduleInterop.SetupVoid(GlobalValues.JS_Register_GoTo_Component_Func, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Show_GoTo_Viewer_Func, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Close_GoTo_Viewer_Func, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_ShowPopover_Func, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Scroll_Marker_Func, _ => true).SetVoidResult();

        // Stands in for the scan of the page that finds the markers.
        moduleInterop.Setup<GoToData[]>(GlobalValues.JS_Get_GoToData_Func, _ => true).SetResult(markersOnPage ?? []);

        var component = context.Render<GoToViewerComponent>(
            builder =>
            {
                parameters?.Invoke(builder);
            });

        return (component, moduleInterop);
    }

    private static string ViewerId(IRenderedComponent<GoToViewerComponent> component)
        => component.Find($".{GlobalValues.GoTo_Viewer_Block_Class}").Id!;

    private static IEnumerable<string> ListButtonTexts(IRenderedComponent<GoToViewerComponent> component)
        => component.FindAll($".{GlobalValues.Goto_Viewer_List_Button}").Select(x => x.TextContent);



    public class Parameters
    {
        [Theory]
        [InlineData("Custom heading", "Custom heading")]
        [InlineData("  Custom heading  ", "Custom heading")]
        public async Task Should_be_able_to_set_and_trim_the_heading_text(string headingText, string expected)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.HeadingText, headingText));

            using (new AssertionScope())
            {
                component.Find($".{GlobalValues.GoTo_Viewer_Header_Class}").TextContent.Should().Be(expected);
                component.Instance.HeadingText.Should().Be(headingText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_use_the_default_heading_text_when_null_empty_or_whitespace(string? headingText)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.HeadingText, headingText));

            component.Find($".{GlobalValues.GoTo_Viewer_Header_Class}").TextContent.Should().Be(GlobalValues.GoTo_Heading_Text);
        }

        [Theory]
        [InlineData("Custom close", "Custom close")]
        [InlineData("  Custom close  ", "Custom close")]
        public async Task Should_be_able_to_set_and_trim_the_close_button_text(string closeButtonText, string expected)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.CloseButtonText, closeButtonText));

            using (new AssertionScope())
            {
                component.Find($".{GlobalValues.GoTo_Viewer_Footer_Button_Class}").TextContent.Should().Be(expected);
                component.Instance.CloseButtonText.Should().Be(closeButtonText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_use_the_default_close_button_text_when_null_empty_or_whitespace(string? closeButtonText)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.CloseButtonText, closeButtonText));

            component.Find($".{GlobalValues.GoTo_Viewer_Footer_Button_Class}").TextContent.Should().Be(GlobalValues.GoTo_Close_Button_Text);
        }

        [Theory]
        [InlineData("Custom message", "Custom message")]
        [InlineData("  Custom message  ", "Custom message")]
        public async Task Should_be_able_to_set_and_trim_the_no_markers_text(string noMarkersText, string expected)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.NoGotoMarkersText, noMarkersText));

            using (new AssertionScope())
            {
                // The message sits on its own line in the markup, so the raw text content includes whitespace.
                component.Find($".{GlobalValues.GoTo_Viewer_Content_Class} > p").TextContent.Trim().Should().Be(expected);
                component.Instance.NoGotoMarkersText.Should().Be(noMarkersText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_use_the_default_no_markers_text_when_null_empty_or_whitespace(string? noMarkersText)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.NoGotoMarkersText, noMarkersText));

            component.Find($".{GlobalValues.GoTo_Viewer_Content_Class} > p").TextContent.Trim().Should().Be(GlobalValues.GoTo_No_Markers_Text);
        }

        [Theory]
        [InlineData("Custom trigger", "Custom trigger")]
        [InlineData("  Custom trigger  ", "Custom trigger")]
        public async Task Should_be_able_to_set_and_trim_the_trigger_button_text(string triggerButtonText, string expected)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.TriggerButtonText, triggerButtonText));

            using (new AssertionScope())
            {
                component.Find($".{GlobalValues.GoTo_Viewer_Trigger_Content_Class}").TextContent.Trim().Should().Be(expected);
                component.Instance.TriggerButtonText.Should().Be(triggerButtonText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_use_the_default_trigger_button_text_when_null_empty_or_whitespace(string? triggerButtonText)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.TriggerButtonText, triggerButtonText));

            component.Find($".{GlobalValues.GoTo_Viewer_Trigger_Content_Class}").TextContent.Trim().Should().Be(GlobalValues.GoTo_Trigger_Button_Text);
        }

        [Fact]
        public async Task Should_include_the_trigger_button_by_default()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context);

            component.FindAll($".{GlobalValues.GoTo_Viewer_Trigger_Block_Class}").Should().HaveCount(1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_exclude_or_include_the_trigger_button(bool excludeTriggerButton)
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.ExcludeTriggerButton, excludeTriggerButton));

            var trigger = component.FindAll($".{GlobalValues.GoTo_Viewer_Trigger_Block_Class}");

            if (true == excludeTriggerButton)
            {
                trigger.Should().BeEmpty();
                return;
            }

            trigger.Should().HaveCount(1);
        }

        [Fact]
        public async Task Should_update_the_heading_text_when_the_value_changes()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, p => p.Add(x => x.HeadingText, "First heading"));

            component.Render(p => p.Add(x => x.HeadingText, "Second heading"));

            component.Find($".{GlobalValues.GoTo_Viewer_Header_Class}").TextContent.Should().Be("Second heading");
        }
    }

    public class Markup
    {
        [Fact]
        public async Task Should_render_the_viewer_as_a_popover_dialog_named_by_its_heading()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context);

            var viewer = component.Find($".{GlobalValues.GoTo_Viewer_Block_Class}");
            var heading = component.Find($".{GlobalValues.GoTo_Viewer_Header_Class}");

            using (new AssertionScope())
            {
                viewer.LocalName.Should().Be("dialog");
                viewer.GetAttribute("popover").Should().Be("auto");
                viewer.GetAttribute("aria-labelledby").Should().Be(heading.Id);
            }
        }

        [Fact]
        public async Task Should_show_the_no_markers_message_and_no_list_when_there_are_no_markers()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context);

            using (new AssertionScope())
            {
                component.FindAll($".{GlobalValues.GoTo_Viewer_List_Class}").Should().BeEmpty();
                component.FindAll($".{GlobalValues.GoTo_Viewer_Content_Class} > p").Should().HaveCount(1);
            }
        }

        [Fact]
        public async Task Should_render_the_trigger_as_a_button_that_does_not_submit_forms()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context);

            var trigger = component.Find($".{GlobalValues.GoTo_Viewer_Trigger_Block_Class}");

            using (new AssertionScope())
            {
                trigger.LocalName.Should().Be("button");
                trigger.GetAttribute("type").Should().Be("button");
            }
        }
    }

    public class Behaviour
    {
        [Fact]
        public async Task Should_register_with_the_javascript_module_on_first_render()
        {
            await using var context = new BunitContext();

            var (component, moduleInterop) = CreateGoToViewer(context);

            component.WaitForAssertion(() =>
            {
                var invocation = moduleInterop.VerifyInvoke(GlobalValues.JS_Register_GoTo_Component_Func);

                using (new AssertionScope())
                {
                    invocation.Arguments[1].Should().Be(nameof(GoToViewerComponent.HandleJSCallBack));
                    invocation.Arguments[2].Should().Be(ViewerId(component));
                }
            });
        }

        [Fact]
        public async Task Should_list_the_markers_and_show_the_viewer_when_the_trigger_button_is_activated()
        {
            await using var context = new BunitContext();

            var (component, moduleInterop) = CreateGoToViewer(context, markersOnPage: _markers);

            await component.Find($".{GlobalValues.GoTo_Viewer_Trigger_Block_Class}").ClickAsync();

            using (new AssertionScope())
            {
                ListButtonTexts(component).Should().Equal("01 - First marker", "02 - Second marker");
                moduleInterop.VerifyInvoke(GlobalValues.JS_Show_GoTo_Viewer_Func).Arguments[0].Should().Be(ViewerId(component));
            }
        }

        [Fact]
        public async Task Should_replace_the_list_each_time_the_trigger_button_is_activated()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context, markersOnPage: _markers);

            await component.Find($".{GlobalValues.GoTo_Viewer_Trigger_Block_Class}").ClickAsync();
            await component.Find($".{GlobalValues.GoTo_Viewer_Trigger_Block_Class}").ClickAsync();

            ListButtonTexts(component).Should().HaveCount(_markers.Length);
        }

        [Fact]
        public async Task Should_list_the_markers_and_show_the_viewer_when_called_from_the_shortcut()
        {
            await using var context = new BunitContext();

            var (component, moduleInterop) = CreateGoToViewer(context);

            await component.InvokeAsync(() => component.Instance.HandleJSCallBack(_markers));

            using (new AssertionScope())
            {
                ListButtonTexts(component).Should().Equal("01 - First marker", "02 - Second marker");
                moduleInterop.VerifyInvoke(GlobalValues.JS_Show_GoTo_Viewer_Func).Arguments[0].Should().Be(ViewerId(component));
            }
        }

        [Fact]
        public async Task Should_replace_the_list_each_time_it_is_called_from_the_shortcut()
        {
            await using var context = new BunitContext();

            var (component, _) = CreateGoToViewer(context);

            await component.InvokeAsync(() => component.Instance.HandleJSCallBack(_markers));
            await component.InvokeAsync(() => component.Instance.HandleJSCallBack([_markers[1]]));

            ListButtonTexts(component).Should().Equal("02 - Second marker");
        }

        [Fact]
        public async Task Should_show_the_chosen_markers_label_when_a_list_button_is_activated()
        {
            await using var context = new BunitContext();

            var (component, moduleInterop) = CreateGoToViewer(context);

            await component.InvokeAsync(() => component.Instance.HandleJSCallBack(_markers));
            await component.FindAll($".{GlobalValues.Goto_Viewer_List_Button}")[1].ClickAsync();

            var invocation = moduleInterop.VerifyInvoke(GlobalValues.JS_ShowPopover_Func);

            using (new AssertionScope())
            {
                invocation.Arguments[0].Should().Be("marker-two");
                invocation.Arguments[1].Should().Be("label-two");
            }
        }

        [Fact]
        public async Task Should_scroll_the_marker_into_view_when_a_list_button_receives_focus()
        {
            await using var context = new BunitContext();

            var (component, moduleInterop) = CreateGoToViewer(context);

            await component.InvokeAsync(() => component.Instance.HandleJSCallBack(_markers));
            await component.FindAll($".{GlobalValues.Goto_Viewer_List_Button}")[0].FocusAsync();

            moduleInterop.VerifyInvoke(GlobalValues.JS_Scroll_Marker_Func).Arguments[0].Should().Be("marker-one");
        }

        [Fact]
        public async Task Should_close_the_viewer_when_the_close_button_is_activated()
        {
            await using var context = new BunitContext();

            var (component, moduleInterop) = CreateGoToViewer(context);

            await component.Find($".{GlobalValues.GoTo_Viewer_Footer_Button_Class}").ClickAsync();

            moduleInterop.VerifyInvoke(GlobalValues.JS_Close_GoTo_Viewer_Func).Arguments[0].Should().Be(ViewerId(component));
        }
    }
}

