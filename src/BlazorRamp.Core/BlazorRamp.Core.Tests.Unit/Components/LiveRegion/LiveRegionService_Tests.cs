using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;


namespace BlazorRamp.Core.Tests.Unit.Components.LiveRegion;

public class LiveRegionService_Tests
{

    public class Announce
    {
        [Fact]
        public async Task Should_pass_announcement_and_invoke_js_module_with_correct_parameters()
        {
            using var context = new BunitContext();
            
            var navManager    = context.Services.GetRequiredService<NavigationManager>();
            var jsInterop     = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var liveService = new LiveRegionService(context.JSInterop.JSRuntime,navManager);

            var announcementText = "Loading";

            var announcement = new Announcement(announcementText, AnnouncementType.OperationStarted, "Save", LiveRegionType.Assertive);

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, announcement)
                            .SetVoidResult();

            await liveService.MakeAnnouncement(announcement);

            using (new AssertionScope())
            {
                var invocations = jsInterop.Invocations;

                invocations.Should().Contain(i => i.Identifier == "import" && i.Arguments[0]!.ToString() == CoreGlobalValues.JS_Live_Region_File_Path);

                moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Live_Region_Announce_Func).Arguments.Should().BeEquivalentTo(new object[] { announcement });
            }

        }

        [Fact]
        public async Task Should_not_pass_announcement_and_invoke_js_module_if_the_announcement_is_null()
        {
            using var context = new BunitContext();

            var navManager    = context.Services.GetRequiredService<NavigationManager>();
            var jsInterop     = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var liveService   = new LiveRegionService(context.JSInterop.JSRuntime, navManager);

            Announcement announcement = default!;

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, announcement)
                            .SetVoidResult();

            await liveService.MakeAnnouncement(null!);

            jsInterop.Invocations.Count.Should().Be(0);

        }



        [Fact]
        public async Task Should_pass_the_live_region_element_reference_correctly()
        {
            using var context = new BunitContext();

            var jsInterop     = context.JSInterop;
            var navManager    = context.Services.GetRequiredService<NavigationManager>();
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);
            var liveService = new LiveRegionService(context.JSInterop.JSRuntime, navManager);

            ElementReference liveRegionRef = default;

            context.Render(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddElementReferenceCapture(1, refCaptured => liveRegionRef = refCaptured);
                builder.CloseElement();
            });

            var announcementText = "Loading";

            var announcement = new Announcement(announcementText, AnnouncementType.OperationStarted, "Save", LiveRegionType.Assertive);

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, announcement)
                            .SetVoidResult();

            await liveService.MakeAnnouncement(announcement);

            var invocations = jsInterop.Invocations;

            using (new AssertionScope())
            {
                invocations.Should().Contain(i => i.Identifier == "import" && i.Arguments[0]!.ToString() == CoreGlobalValues.JS_Live_Region_File_Path);

                moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Live_Region_Announce_Func).Arguments.Should().BeEquivalentTo(new object[] { announcement});
            }

        }
    }


    public class NavigationManager_LocationChanged()
    {

        [Fact]
        public void Should_reinitialize_js_module_when_navigation_occurs()
        {
            using var context = new BunitContext();

            var navManager    = context.Services.GetRequiredService<NavigationManager>();
            var jsInterop     = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Live_Region_Check_Close_Popover_Func)
                .SetVoidResult();

            var liveService = new LiveRegionService(jsInterop.JSRuntime, navManager);

            navManager.NavigateTo("/new-page");

            //await Task.Delay(100, CancellationToken.None);

            using (new AssertionScope())
            {
                jsInterop.Invocations.Should().Contain(i => i.Identifier == "import" && i.Arguments[0]!.ToString() == CoreGlobalValues.JS_Live_Region_File_Path);

                moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Live_Region_Check_Close_Popover_Func);
            }
        }
    }


    public class DisposeAsync
    {
        [Fact]
        public async Task Should_be_idempotent_and_not_throw_if_called_more_than_once()
        {
            using var context = new BunitContext();

            var jsInterop   = context.JSInterop;
            var navManager  = context.Services.GetRequiredService<NavigationManager>();
            var liveService = new LiveRegionService(context.JSInterop.JSRuntime, navManager);

            await liveService.DisposeAsync();

            await FluentActions.Invoking(async () => await liveService.DisposeAsync()).Should().NotThrowAsync();

        }
        [Fact]
        public async Task Should_not_throw_if_the_module_is_null()
        {
            using var context = new BunitContext();
            var navManager    = context.Services.GetRequiredService<NavigationManager>();
            var liveService   = new LiveRegionService(null!, navManager);

            await FluentActions.Awaiting(() => liveService.DisposeAsync().AsTask()).Should().NotThrowAsync();
        }
        [Fact]
        public async Task Should_dispose_js_module_when_dispose_is_called()
        {
            using var context = new BunitContext();

            var navManager    = context.Services.GetRequiredService<NavigationManager>();
            var jsInterop     = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);

            var liveService = new LiveRegionService(jsInterop.JSRuntime, navManager);

            var announcement = new Announcement("Test", AnnouncementType.OperationStarted, "Test", LiveRegionType.Polite);

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Live_Region_Announce_Func, announcement)
                .SetVoidResult();

            await liveService.MakeAnnouncement(announcement);

            await liveService.DisposeAsync();

            await FluentActions.Invoking(async () => await liveService.DisposeAsync()).Should().NotThrowAsync();
        }
    }
}
