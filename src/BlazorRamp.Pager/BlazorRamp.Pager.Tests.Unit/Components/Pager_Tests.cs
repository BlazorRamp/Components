using BlazorRamp.Core.Services;
using BlazorRamp.Pager.Common.Constants;
using BlazorRamp.Pager.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.Pager.Tests.Unit.Components;

public class Pager_Tests
{
    public static IRenderedComponent<BlazorRamp.Pager.Components.Pager> CreatePager(BunitContext context,
        Action<ComponentParameterCollectionBuilder<BlazorRamp.Pager.Components.Pager>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

        moduleInterop.SetupVoid(GlobalValues.JS_Register_Focus_In_Callback, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Unregister_Focus_In_Callback, _ => true).SetVoidResult();

        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var component = context.Render<BlazorRamp.Pager.Components.Pager>(builder => parameters?.Invoke(builder));

        return component;
    }


    public class Parameters
    {
        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_be_able_to_select_a_button_or_link_pager(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType));

            var element = pager.Find($".{@GlobalValues.Pager_Items_Class}").FirstChild!;

            if (selectorType == PagerSelectorType.Link)
            {
                element.NodeName.ToLower().Should().Be("a");
                return;
            }

            element.NodeName.ToLower().Should().Be("button");

        }

        [Theory]
        [InlineData(PageAlignment.Start)]
        [InlineData(PageAlignment.End)]
        [InlineData(PageAlignment.Centred)]
        public async Task Should_be_able_to_set_the_pager_alignment_which_defaults_to_centre(PageAlignment alignment)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.PageAlignment, alignment));

            var classList = pager.Find("nav").ClassList;

            using (new AssertionScope())
            {
                classList[0].Should().Be(GlobalValues.Pager_Class);

                switch (alignment)
                {
                    case PageAlignment.Start:
                        classList.Count.Should().Be(2);
                        classList[1].Should().Be(GlobalValues.Pager_Align_Start_Modifier);
                        break;
                    case PageAlignment.End:
                        classList.Count.Should().Be(2);
                        classList[1].Should().Be(GlobalValues.Pager_Align_End_Modifier);
                        break;
                    default:
                        classList.Count.Should().Be(1);
                        break;
                }
            }

        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My Pager")]
        public async Task Should_be_able_to_set_the_aria_label_for_the_pager_that_defaults_if_null_empty_or_whitespace(string? ariaLabel)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.AriaLabel, ariaLabel));

            var labelSpan = pager.Find("nav > span").TextContent;

            if (String.IsNullOrWhiteSpace(ariaLabel))
            {
                labelSpan.Should().Be(GlobalValues.Pager_Aria_Label);
                return;
            }
            labelSpan.Should().Be(ariaLabel);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My First")]
        public async Task Should_be_able_to_set_the_text_for_the_first_selector_which_defaults_if_null_empty_or_whitespace(string? selectorText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.FirstText, selectorText));
            var selectors = pager.Find($".{GlobalValues.Pager_Items_Class}").Children;

            using (new AssertionScope())
            {
                var textSpan = selectors[0].LastElementChild;//text span is after the icons
                if (String.IsNullOrWhiteSpace(selectorText))
                {
                    textSpan?.TextContent.Should().Be(GlobalValues.Pager_Selector_First_Text);
                    return;
                }
                textSpan?.TextContent.Should().Be(selectorText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My Previous")]
        public async Task Should_be_able_to_set_the_text_for_the_previous_selector_which_defaults_if_null_empty_or_whitespace(string? selectorText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.PreviousText, selectorText));
            var selectors = pager.Find($".{GlobalValues.Pager_Items_Class}").Children;

            using (new AssertionScope())
            {
                var textSpan = selectors[1].LastElementChild;//text span is after the icons
                if (String.IsNullOrWhiteSpace(selectorText))
                {
                    textSpan?.TextContent.Should().Be(GlobalValues.Pager_Selector_Prev_Text);
                    return;
                }
                textSpan?.TextContent.Should().Be(selectorText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My Next")]
        public async Task Should_be_able_to_set_the_text_for_the_next_selector_which_defaults_if_null_empty_or_whitespace(string? selectorText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.NextText, selectorText));
            var selectors = pager.Find($".{GlobalValues.Pager_Items_Class}").Children;

            using (new AssertionScope())
            {
                var textSpan = selectors[2].FirstElementChild;//text span is before the icons
                if (String.IsNullOrWhiteSpace(selectorText))
                {
                    textSpan?.TextContent.Should().Be(GlobalValues.Pager_Selector_Next_Text);
                    return;
                }
                textSpan?.TextContent.Should().Be(selectorText);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My Last")]
        public async Task Should_be_able_to_set_the_text_for_the_last_selector_which_defaults_if_null_empty_or_whitespace(string? selectorText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.LastText, selectorText));
            var selectors = pager.Find($".{GlobalValues.Pager_Items_Class}").Children;

            using (new AssertionScope())
            {
                var textSpan = selectors[3].FirstElementChild;//text span is before the icons
                if (String.IsNullOrWhiteSpace(selectorText))
                {
                    textSpan?.TextContent.Should().Be(GlobalValues.Pager_Selector_Last_Text);
                    return;
                }
                textSpan?.TextContent.Should().Be(selectorText);
            }

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_so_show_or_hide_the_first_and_last_buttons(bool showFirstLast)
        {
            await using var context = new BunitContext();
            var pager = CreatePager(context, p => p.Add(x => x.ShowFirstLast, showFirstLast));

            var selectors = pager.Find($".{GlobalValues.Pager_Items_Class}").Children;

            using (new AssertionScope())
            {
                if (true == showFirstLast)
                {
                    selectors[0].LastElementChild!.TextContent.Should().Be(GlobalValues.Pager_Selector_First_Text);
                    selectors[3].FirstElementChild!.TextContent.Should().Be(GlobalValues.Pager_Selector_Last_Text);
                    return;
                }

                selectors[0].LastElementChild!.TextContent.Should().Be(GlobalValues.Pager_Selector_Prev_Text);
                selectors[1].FirstChild!.TextContent.Should().Be(GlobalValues.Pager_Selector_Next_Text);
            }

        }

        [Theory]
        [InlineData(100, 10)]
        [InlineData(100, 5)]
        [InlineData(100, 1)]
        public async Task Total_item_count_should_be_split_by_the_items_per_page(int totalItemCount, int itemsPerPage)
        {
            await using var context = new BunitContext();

            var pageCountText = "Last Page {lastpage}";

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, totalItemCount)
                                                   .Add(x => x.ItemsPerPage, itemsPerPage)
                                                   .Add(x => x.CurrentItemCount, totalItemCount)
                                                   .Add(x => x.PageCountText, pageCountText));

            int totalPages = (int)Math.Ceiling((double)totalItemCount / itemsPerPage);
            var resultText = pageCountText.Replace("{lastpage}", totalPages.ToString());

            pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Be(resultText);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("No Records")]
        public async Task Should_be_able_to_set_the_ne_record_text_which_defaults_if_null_empty_or_whitespace(string? noRecordText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, 0)
                                                   .Add(x => x.NoRecordsText, noRecordText));

            if (String.IsNullOrWhiteSpace(noRecordText))
            {
                pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Be(GlobalValues.Pager_No_Records_Text);
                return;
            }
            pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Be(noRecordText);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My page {currentpage} {lastpage} {startrow} {endrow}")]
        public async Task Should_be_able_to_set_the_ne_page_count_text_which_defaults_if_null_empty_or_whitespace(string? pageCountText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.PageCountText, pageCountText));

            if (String.IsNullOrWhiteSpace(pageCountText))
            {
                var defaultText = GlobalValues.Pager_Count_Text.Replace("{currentpage}", 1.ToString()).Replace("{lastpage}", 10.ToString())
                                               .Replace("{startrow}", 1.ToString()).Replace("{endrow}", 10.ToString());

                pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Be(defaultText);
                return;
            }

            var resultsText = pageCountText.Replace("{currentpage}", 1.ToString()).Replace("{lastpage}", 10.ToString())
                              .Replace("{startrow}", 1.ToString()).Replace("{endrow}", 10.ToString());

            pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Be(resultsText);

        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My filtered rows {filteredrows} {totalrows}")]
        public async Task Should_be_able_to_set_the_ne_filter_count_text_which_defaults_if_null_empty_or_whitespace(string? filteredCountText)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentItemCount, 80)
                                                   .Add(x => x.FilterCountText, filteredCountText));

            if (String.IsNullOrWhiteSpace(filteredCountText))
            {
                var defaultText = GlobalValues.Pager_Filter_Count_Text.Replace("{filteredrows}", 80.ToString()).Replace("{totalrows}", 100.ToString());

                pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Contain(defaultText);
                return;
            }

            var resultsText = filteredCountText.Replace("{filteredrows}", 80.ToString()).Replace("{totalrows}", 100.ToString());


            pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Contain(resultsText);

        }


        [Theory]
        [InlineData(1, 100, 10)]
        [InlineData(5, 100, 10)]
        [InlineData(10, 100, 10)]
        public async Task Current_page_should_update_the_pager_information(int currentPage, int totalItemCount, int itemsPerPage)
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, totalItemCount)
                                                   .Add(x => x.ItemsPerPage, itemsPerPage)
                                                   .Add(x => x.CurrentItemCount, totalItemCount)
                                                   .Add(x => x.CurrentPage, currentPage));

            int totalPages = (int)Math.Ceiling((double)totalItemCount / itemsPerPage);
            var rowStart = (10 * currentPage) - (10 - 1);
            var rowEnd = 10 * currentPage;

            var pageCountText = GlobalValues.Pager_Count_Text.Replace("{currentpage}", currentPage.ToString()).Replace("{lastpage}", totalPages.ToString())
                                   .Replace("{startrow}", rowStart.ToString()).Replace("{endrow}", rowEnd.ToString());

            pager.Find($".{GlobalValues.Pager_Information_Class}").TextContent.Should().Be(pageCountText);

        }

    }

    public class RequestPageChange
    {
        private static string ModifierClassFor(PagerSelectorType selectorType, string buttonModifier, string linkModifier)

            => selectorType == PagerSelectorType.Link ? linkModifier : buttonModifier;

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_invoke_current_page_changed_with_the_next_page_when_next_is_clicked_and_not_on_the_last_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 5)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var nextModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_Next_Modifier, GlobalValues.Pager_Link_Next_Modifier);

            pager.Find($".{nextModifier}").Click();

            capturedPage.Should().Be(6);
        }

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_not_invoke_current_page_changed_when_next_is_clicked_on_the_last_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 10)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var nextModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_Next_Modifier, GlobalValues.Pager_Link_Next_Modifier);

            pager.Find($".{nextModifier}").Click();

            capturedPage.Should().BeNull();
        }

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_not_invoke_current_page_changed_when_previous_is_clicked_on_the_first_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 1)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var previousModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_Previous_Modifier, GlobalValues.Pager_Link_Previous_Modifier);

            pager.Find($".{previousModifier}").Click();

            capturedPage.Should().BeNull();
        }

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_invoke_current_page_changed_with_page_one_when_first_is_clicked_and_not_already_on_the_first_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 5)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var firstModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_First_Modifier, GlobalValues.Pager_Link_First_Modifier);

            pager.Find($".{firstModifier}").Click();

            capturedPage.Should().Be(1);
        }

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_not_invoke_current_page_changed_when_first_is_clicked_on_the_first_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 1)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var firstModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_First_Modifier, GlobalValues.Pager_Link_First_Modifier);

            pager.Find($".{firstModifier}").Click();

            capturedPage.Should().BeNull();
        }

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_invoke_current_page_changed_with_the_last_page_when_last_is_clicked_and_not_already_on_the_last_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 5)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var lastModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_Last_Modifier, GlobalValues.Pager_Link_Last_Modifier);

            pager.Find($".{lastModifier}").Click();

            capturedPage.Should().Be(10);
        }

        [Theory]
        [InlineData(PagerSelectorType.Button)]
        [InlineData(PagerSelectorType.Link)]
        public async Task Should_not_invoke_current_page_changed_when_last_is_clicked_on_the_last_page(PagerSelectorType selectorType)
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.PagerSelectorType, selectorType)
                                                   .Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 10)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            var lastModifier = ModifierClassFor(selectorType, GlobalValues.Pager_Button_Last_Modifier, GlobalValues.Pager_Link_Last_Modifier);

            pager.Find($".{lastModifier}").Click();

            capturedPage.Should().BeNull();
        }

        [Fact]
        public async Task Should_block_all_navigation_when_there_are_no_records_even_though_the_disabled_check_is_driven_by_row_counts()
        {
            await using var context = new BunitContext();

            int? capturedPage = null;

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, 0)
                                                   .Add(x => x.CurrentItemCount, 0)
                                                   .Add(x => x.CurrentPageChanged, EventCallback.Factory.Create<int>(context, v => capturedPage = v)));

            // _lastPage falls back to 1 and _currentPage clamps to 1 when there are no rows, so every
            // selector's own boundary check in RequestPageChange blocks it independently of
            // CheckSetDisableButton's row-count check — this test exists to pin that down explicitly,
            // since the two checks use different inputs and could drift apart in future changes.
            pager.Find($".{GlobalValues.Pager_Button_Next_Modifier}").Click();
            capturedPage.Should().BeNull();

            pager.Find($".{GlobalValues.Pager_Button_Last_Modifier}").Click();
            capturedPage.Should().BeNull();
        }

        [Fact]
        public async Task Should_not_throw_when_clicked_and_current_page_changed_has_no_delegate_attached()
        {
            await using var context = new BunitContext();

            var pager = CreatePager(context, p => p.Add(x => x.TotalItemCount, 100)
                                                   .Add(x => x.CurrentItemCount, 100)
                                                   .Add(x => x.ItemsPerPage, 10)
                                                   .Add(x => x.CurrentPage, 5));

            var act = () => pager.Find($".{GlobalValues.Pager_Button_Next_Modifier}").Click();

            act.Should().NotThrow();
        }
    }

}


