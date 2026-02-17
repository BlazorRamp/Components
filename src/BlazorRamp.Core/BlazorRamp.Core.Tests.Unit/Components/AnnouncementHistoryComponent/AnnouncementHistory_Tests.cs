using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Core.Components.AnnouncementHistory;
using BlazorRamp.Core.Services;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;


namespace BlazorRamp.Core.Tests.Unit.Components.AnnouncementHistoryComponent;

public class AnnouncementHistory_Tests
{
    public class Parameters
    {
        private static IRenderedComponent<AnnouncementHistory> CreateAnnouncementHistoryWithoutParams(BunitContext context)
        {
            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();
            context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);

            return context.Render<AnnouncementHistory>();

        }
        private static IRenderedComponent<AnnouncementHistory> CreateAnnouncementHistoryWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
        {
            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();
            context.JSInterop.SetupModule(CoreGlobalValues.JS_Live_Region_File_Path);

            return context.Render<AnnouncementHistory>(paramBuilder => paramBuilder.TryAdd(paramName, paramValue));
        }

        [Theory]
        [InlineData("Some title")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Title_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? title)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = (title == "missing") ? CreateAnnouncementHistoryWithoutParams(context)
                                                       : CreateAnnouncementHistoryWithParamByName<string>(context,nameof(AnnouncementHistory.Title), title!);

            var contextText = announcementHistory.Find("#" + @CoreGlobalValues.AH_Title_ID).TextContent;
 
            switch(title)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(CoreGlobalValues.AH_Text_For_Heading);
                    break;
                default:
                    contextText.Should().Be(title!.Trim());
                    break;
            }

        }

        [Theory]
        [InlineData("Close Text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Close_text_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? closeText)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = (closeText == "missing") ? CreateAnnouncementHistoryWithoutParams(context)
                                                       : CreateAnnouncementHistoryWithParamByName<string>(context, nameof(AnnouncementHistory.CloseText), closeText!);

            var contextText = announcementHistory.Find("#" + @CoreGlobalValues.AH_Close_Button_ID).TextContent;

            switch (closeText)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(CoreGlobalValues.AH_Text_For_Close_Btn);
                    break;
                default:
                    contextText.Should().Be(closeText!.Trim());
                    break;
            }

        }

        [Theory]
        [InlineData("Clear Text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Clear_and_close_text_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? clearText)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = (clearText == "missing") ? CreateAnnouncementHistoryWithoutParams(context)
                                                       : CreateAnnouncementHistoryWithParamByName<string>(context, nameof(AnnouncementHistory.ClearCloseText), clearText!);

            var contextText = announcementHistory.Find("#" + @CoreGlobalValues.AH_Clear_Button_ID).TextContent;

            switch (clearText)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(CoreGlobalValues.AH_Text_For_Clear_Btn);
                    break;
                default:
                    contextText.Should().Be(clearText!.Trim());
                    break;
            }
        }

        [Theory]
        [InlineData("Refresh Text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Refresh_text_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? refreshText)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = (refreshText == "missing") ? CreateAnnouncementHistoryWithoutParams(context)
                                                       : CreateAnnouncementHistoryWithParamByName<string>(context, nameof(AnnouncementHistory.RefreshText), refreshText!);

            var contextText = announcementHistory.Find("#" + @CoreGlobalValues.AH_Refresh_Button_ID).TextContent;

            switch (refreshText)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(CoreGlobalValues.AH_Text_For_Refresh_Btn);
                    break;
                default:
                    contextText.Should().Be(refreshText!.Trim());
                    break;
            }

        }

        [Theory]
        [InlineData("No Data Text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task No_data_text_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? noDataText)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = (noDataText == "missing") ? CreateAnnouncementHistoryWithoutParams(context)
                                                       : CreateAnnouncementHistoryWithParamByName<string>(context, nameof(AnnouncementHistory.NoDataText), noDataText!);


            var contextText = announcementHistory.Find($"#{@CoreGlobalValues.AH_Content_ID} > p").TextContent;

            switch (noDataText)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(CoreGlobalValues.AH_Text_No_Content);
                    break;
                default:
                    contextText.Should().Be(noDataText!.Trim());
                    break;
            }

        }

        [Theory]
        [InlineData("Trigger Text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        [InlineData("missing")]
        public async Task Trigger_text_should_be_used_when_not_null_or_whitespace_othersise_a_default_is_used(string? triggerText)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = (triggerText == "missing") ? CreateAnnouncementHistoryWithoutParams(context)
                                                       : CreateAnnouncementHistoryWithParamByName<string>(context, nameof(AnnouncementHistory.TriggerText), triggerText!);

            var contextText = announcementHistory.Find($"#{@CoreGlobalValues.AH_Trigger_Button_ID} > span:last-child").TextContent;

            switch (triggerText)
            {
                case null:
                case string value when String.IsNullOrWhiteSpace(value) || value == "missing":
                    contextText.Should().Be(CoreGlobalValues.AH_Text_For_Trigger_Btn);
                    break;
                default:
                    contextText.Should().Be(triggerText!.Trim());
                    break;
            }

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Trigger_classes_should_be_set_based_on_the_trigger_visible_value(bool triggerVisible)
        {
            await using var context = new BunitContext();
            IRenderedComponent<AnnouncementHistory> announcementHistory;

            announcementHistory = CreateAnnouncementHistoryWithParamByName<bool>(context, nameof(AnnouncementHistory.TriggerVisible), triggerVisible);

            var triggerClasses = triggerVisible ? CoreGlobalValues.AH_Trigger_Class : CoreUtilities.CreateClassList(CoreGlobalValues.AH_Trigger_Class, CoreGlobalValues.AH_Trigger_Modifier);

            var classValues = announcementHistory.Find($"#{@CoreGlobalValues.AH_Trigger_Button_ID}").ClassName;

            classValues.Should().Be(triggerClasses);

        }
    }
}
