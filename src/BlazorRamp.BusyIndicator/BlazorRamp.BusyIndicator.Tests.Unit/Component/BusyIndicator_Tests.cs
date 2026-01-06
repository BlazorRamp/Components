using BlazorRamp.BusyIndicator.Common.Constants;
using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Core.Services;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using static BlazorRamp.BusyIndicator.Components.BusyIndicator;
using BusyIndicatorComponent = BlazorRamp.BusyIndicator.Components.BusyIndicator;



namespace BlazorRamp.BusyIndicator.Tests.Unit.Component;

public class BusyIndicator_Tests
{
    public class Parameters
    {
        private static IRenderedComponent<BusyIndicatorComponent> CreateBusyIndicatorWithoutParams(BunitContext context)
        {
            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();
            context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

            return context.Render<BusyIndicatorComponent>();

        }
        private static IRenderedComponent<BusyIndicatorComponent> CreateBusyIndicatorWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, bool showIndicator = false)
        {
            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();
            context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

            return context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, showIndicator)
                                                          .TryAdd(paramName, paramValue));
        }

        [Theory]
        [InlineData(StyleAs.Dynamic)]
        [InlineData(StyleAs.OnLight)]
        [InlineData(StyleAs.OnDark)]
        [InlineData(null)]
        public async Task Style_as_should_use_the_value_provided_with_a_default_of_dynamic_which_removes_the_data_attribute(StyleAs? styleAs)
        {
            await using var context = new BunitContext();

            IRenderedComponent<BusyIndicatorComponent> busyIndicator;

            busyIndicator = (styleAs is null) ? busyIndicator = CreateBusyIndicatorWithoutParams(context)
                                              : CreateBusyIndicatorWithParamByName<StyleAs>(context, nameof(BusyIndicatorComponent.StyleAs), styleAs.Value);

            if (styleAs is not null && styleAs != StyleAs.Dynamic)
            {
                string styleAsValue = $"[data-br-style={CoreUtilities.GetStyleAsValue(styleAs.Value)}]";

                busyIndicator.FindAll(styleAsValue).Should().NotBeEmpty();

                return;
            }

            busyIndicator.FindAll("[data-br-style]").Should().BeEmpty();
        }

        [Theory]
        [InlineData("My Indicator")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Indicator_Label_when_not_missing_null_empty_or_whitespace_should_be_used_otherwise_the_default_is_used(string? indicatorLabel)
        {
            await using var context = new BunitContext();

            IRenderedComponent<BusyIndicatorComponent> busyIndicator;

            busyIndicator = (indicatorLabel == "missing") ? busyIndicator = CreateBusyIndicatorWithoutParams(context)
                                                          : CreateBusyIndicatorWithParamByName<string>(context, nameof(BusyIndicatorComponent.IndicatorLabel), indicatorLabel!);

            if (true == String.IsNullOrWhiteSpace(indicatorLabel) || indicatorLabel == "missing")
            {
                busyIndicator.Find("div > span.br-visually-hidden").TextContent.Should().Be(GlobalValues.Busy_Indicator_Label);
                return;
            }

            busyIndicator.Find("div > span.br-visually-hidden").TextContent.Should().Be(indicatorLabel);
        }

        [Theory]
        [InlineData("Busy Please Wait")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Busy_text_when_not_missing_null_empty_whitespace_should_be_used_otherwise_empty_is_used(string? busyText)
        {
            await using var context = new BunitContext();

            IRenderedComponent<BusyIndicatorComponent> busyIndicator;

            busyIndicator = (busyText == "missing") ? CreateBusyIndicatorWithoutParams(context) 
                                                    : CreateBusyIndicatorWithParamByName<string>(context, nameof(BusyIndicatorComponent.BusyText), busyText!);

            using (new AssertionScope())
            {
                if (true == String.IsNullOrWhiteSpace(busyText) || busyText == "missing")
                {
                    busyIndicator.FindAll($"div.{GlobalValues.Busy_Text_Class}").Should().BeEmpty();
                    busyIndicator.Instance.BusyText.Should().BeNullOrWhiteSpace();
                    return;
                }

                busyIndicator.FindAll($"div.{GlobalValues.Busy_Text_Class}").Should().NotBeEmpty();
                busyIndicator.Instance.BusyText.Should().Be(busyText);
            }
        }

        
        [Theory]
        [InlineData("Some trigger")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Indicator_trigger_should_be_used_when_not_missing_null_empty_whitespace_should_be_used_otherwise_empty_is_used(string? trigger)
        {
            await using var context = new BunitContext();

            IRenderedComponent<BusyIndicatorComponent> busyIndicator;

            busyIndicator = (trigger == "missing") ? CreateBusyIndicatorWithoutParams(context)
                                                    : CreateBusyIndicatorWithParamByName<string>(context, nameof(BusyIndicatorComponent.IndicatorTrigger), trigger!);

            using (new AssertionScope())
            {
                if (true == String.IsNullOrWhiteSpace(trigger) || trigger == "missing")
                {
                    busyIndicator.Instance.IndicatorTrigger.Should().BeNullOrWhiteSpace();
                    return;
                }

                busyIndicator.Instance.IndicatorTrigger.Should().Be(trigger);
            }
        }

        [Fact]
        public async Task Overlay_position_should_use_the_pre_set_value_if_not_provided()
        {
            await using var context = new BunitContext();
            var busyIndicator = CreateBusyIndicatorWithoutParams(context);

            using(new AssertionScope())
            {
                busyIndicator.Find($"div.{GlobalValues.Busy_Class}").ClassList.Should().Contain(GlobalValues.Busy_Fix_At_Container_Modifier);///value only when container the default
                busyIndicator.Instance.OverlayPosition.Should().Be(OverlayPosition.Container);
            }
        }
        [Fact]
        public async Task Overlay_position_should_use_the_provided_value()
        {
            await using var context = new BunitContext();
            var busyIndicator = CreateBusyIndicatorWithParamByName<OverlayPosition>(context, nameof(BusyIndicatorComponent.OverlayPosition), OverlayPosition.Screen);

            using (new AssertionScope())
            {
                busyIndicator.Find($"div.{GlobalValues.Busy_Class}").ClassList.Should().NotContain(GlobalValues.Busy_Fix_At_Container_Modifier);//value only when container the default
                busyIndicator.Instance.OverlayPosition.Should().Be(OverlayPosition.Screen);
            }
        }

        [Fact]
        public async Task Content_position_should_use_the_pre_set_value_if_not_provided()
        {
            await using var context = new BunitContext();
            var busyIndicator = CreateBusyIndicatorWithoutParams(context);

            using (new AssertionScope())
            {
                busyIndicator.Find($"div.{GlobalValues.Busy_Class}").ClassList.Should().NotContain(GlobalValues.Busy_Centred_Modifier);//should not have this when top is used
                busyIndicator.Instance.ContentPosition.Should().Be(ContentPosition.Top);
            }
        }
        [Fact]
        public async Task Content_position_should_use_the_provided_value()
        {
            await using var context = new BunitContext();
            var busyIndicator = CreateBusyIndicatorWithParamByName<ContentPosition>(context, nameof(BusyIndicatorComponent.ContentPosition), ContentPosition.Centre);

            using (new AssertionScope())
            {
                busyIndicator.Find($"div.{GlobalValues.Busy_Class}").ClassList.Should().Contain(GlobalValues.Busy_Centred_Modifier);
                busyIndicator.Instance.ContentPosition.Should().Be(ContentPosition.Centre);
            }
        }
        [Fact]
        public async Task Display_time_out_ms_should_use_the_pre_set_value_if_not_provided()
        {
            await using var context = new BunitContext();

            CreateBusyIndicatorWithoutParams(context).Instance.DisplayTimeoutMS.Should().Be(GlobalValues.Busy_Indicator_Timeout_MS);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Display_time_out_ms_should_use_the_provided_value_if_greater_than_zero_otherwise_it_should_use_the_default(int timeOut)
        {
            await using var context = new BunitContext();

            var coreModule = context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            var busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.DisplayTimeoutMS, timeOut));

            _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            _ = coreModule.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, _ => true).SetVoidResult();

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            var expectedValue = timeOut < 1 ? GlobalValues.Busy_Indicator_Timeout_MS : timeOut;

            using (new AssertionScope())
            {
                busyIndicator.Instance.DisplayTimeoutMS.Should().Be(timeOut);

                var invocation = busyModule.VerifyInvoke(GlobalValues.JS_Start_Busy_Indicator);

                invocation.Arguments[2].Should().Be(expectedValue);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("missing")]
        [InlineData("Starting")]
        public async Task Aria_start_text_should_only_be_used_if_not_null_empty_or_missing(string? ariaStartText)
        {
            await using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var coreModule = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = jsInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            IRenderedComponent<BusyIndicatorComponent> busyIndicator;

            _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            _ = coreModule.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, _ => true).SetVoidResult();

            var announcement = new Announcement(ariaStartText!, AnnouncementType.OperationStarted, "", LiveRegionType.Assertive);

            busyIndicator = (ariaStartText == "missing")
                            ? busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false))
                            : busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.AriaStartText, ariaStartText).Add(p => p.ShowIndicator, false));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            using (new AssertionScope())
            {
                if (!String.IsNullOrWhiteSpace(ariaStartText) && ariaStartText != "missing")
                {
                    var invocation = coreModule.VerifyInvoke(CoreGlobalValues.JS_Live_Region_Announce_Func);

                    invocation.Arguments[0].Should().Be(announcement);

                    return;
                }

               coreModule.VerifyNotInvoke(CoreGlobalValues.JS_Live_Region_Announce_Func);
            }
        }

       

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("missing")]
        public async Task Aria_end_text_should_use_the_pre_set_value_if_null_empty_or_missing(string? ariaEndText)
        {
            await using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var coreModule = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = jsInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            var announcement = new Announcement(GlobalValues.Busy_Indicator_End_Text, AnnouncementType.SystemError, "", LiveRegionType.Assertive);

            IRenderedComponent<BusyIndicatorComponent> busyIndicator;

           _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            _ = coreModule.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, _ => true).SetVoidResult();

            busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false));
            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            if (ariaEndText == "missing")
            {
                busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false).Add(p => p.EndStatus, AnnouncementType.SystemError));
            }
            else
            {
                busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.AriaEndText, ariaEndText).Add(p => p.ShowIndicator, false).Add(p => p.EndStatus, AnnouncementType.SystemError)); 
            }
            

            using (new AssertionScope())
            {
                var invocation = coreModule.VerifyInvoke(CoreGlobalValues.JS_Live_Region_Announce_Func);

                invocation.Arguments[0].Should().Be(announcement);

            }
        }


        [Fact]
        public async Task Aria_end_text_should_be_used_if_not_null_or_empty()
        {
            await using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var coreModule = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = jsInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            var ariaEndText = "Task Finished";

            var announcement = new Announcement(ariaEndText, AnnouncementType.OperationCompleted, "", LiveRegionType.Assertive);

            _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            _ = coreModule.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, _ => true).SetVoidResult();

            var busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.AriaEndText, ariaEndText).Add(p => p.ShowIndicator, false));

            busyIndicator.Render(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));
            busyIndicator.Render(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false).Add(p => p.EndStatus, AnnouncementType.OperationCompleted));

            using (new AssertionScope())
            {
                busyIndicator.Instance.AriaEndText.Should().Be(ariaEndText);

                var invocations = coreModule.VerifyInvoke(CoreGlobalValues.JS_Live_Region_Announce_Func,1);

                invocations[0].Arguments[0].Should().Be(announcement);
            }
        }

        [Fact]
        public async Task Child_content_should_be_used_if_not_null_or_missing()
        {
            await using var context = new BunitContext();

            var coreModule = context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            var busyIndicator = context.Render<BusyIndicatorComponent>(parameters => parameters.AddChildContent("<h1>Test Content</h1>"));

            busyIndicator.Markup.Should().Contain("<h1>Test Content</h1>");
        }

        [Fact]
        public async Task On_parameters_set_should_return_when_show_indicator_is_unchanged()
        {
            await using var context = new BunitContext();

            var coreModule = context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            
            var busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            using (new AssertionScope())
            {
                busyModule.VerifyInvoke(GlobalValues.JS_Start_Busy_Indicator, 1);
            }
        }


        [Fact]
        public async Task Show_indicator_when_changed_to_false_should_call_stop_busy_indicator_if_it_open()
        {
            await using var context = new BunitContext();

            var coreModule = context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

           context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            _ = coreModule.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, _ => true).SetVoidResult();
            
            var stopBusy = busyModule.SetupVoid(GlobalValues.JS_Stop_Busy_Indicator, _ => true).SetVoidResult();

            var busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false));

            await Task.Delay((CoreGlobalValues.Live_Region_Delay_MS * 2) + 100);//needs to wait just like the component before calling stop.

            stopBusy.VerifyInvoke(GlobalValues.JS_Stop_Busy_Indicator);

        }

    }

    public class OnBusyCompleted 
    {
        [Fact]
        public async Task Should_be_invoked_when_busy_indicator_stops()
        {
            await using var context = new BunitContext();

            var coreModule = context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var busyModule = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

            _ = busyModule.SetupVoid(GlobalValues.JS_Start_Busy_Indicator, _ => true).SetVoidResult();
            _ = coreModule.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, _ => true).SetVoidResult();
            _ = busyModule.SetupVoid(GlobalValues.JS_Stop_Busy_Indicator, _ => true).SetVoidResult();

            var callbackInvoked = false;

            var busyIndicator = context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false).Add(p => p.OnBusyCompleted, () => { callbackInvoked = true; }));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, true));

            busyIndicator.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, false));


            await Task.Delay((CoreGlobalValues.Live_Region_Delay_MS * 2) + 150);//needs to wait just like the component before calling stop.

            callbackInvoked.Should().BeTrue();
        }
    }

}

