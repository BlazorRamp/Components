using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Models;
using BlazorRamp.DataTable.Common.Utilities;
using BlazorRamp.DataTable.Components;
using BlazorRamp.DataTable.Tests.Unit.SharedTestModels;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class DataTable_Parameter_Tests
{
    public static IRenderedComponent<DataTable<Contact>> CreateDataTable(BunitContext context,
       Action<ComponentParameterCollectionBuilder<DataTable<Contact>>>? parameters = null)
    {
        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var busyModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.BusyIndicator/assets/js/busy-indicator.js");
        var coreModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.Core/assets/js/core-live-region.js");
        var tableModuleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);

        tableModuleInterop.SetupVoid();
        busyModuleInterop.SetupVoid();
        coreModuleInterop.SetupVoid();
        coreModuleInterop.SetupVoid("announcement", _ => true).SetVoidResult();
        var component = context.Render<DataTable<Contact>>(builder =>
        {
            builder.Add(x => x.TableColumns, DefaultTableColumns);
            parameters?.Invoke(builder);
        });

        return component;
    }

    public static RenderFragment DefaultTableColumns => builder =>
    {
        Expression<Func<Contact, object>> givenName = c => c.GivenName;
        Expression<Func<Contact, object>> rate = c => c.Rate;

        builder.OpenComponent<DataColumn<Contact>>(0);
        builder.AddAttribute(1, "DataProperty", givenName);
        builder.AddAttribute(2, "DisplayName", "First name");
        builder.AddAttribute(3, "IsSortable", true);
        builder.CloseComponent();

        builder.OpenComponent<DataColumn<Contact>>(4);
        builder.AddAttribute(5, "DataProperty", rate);
        builder.AddAttribute(6, "DisplayName", "Rate");
        builder.AddAttribute(7, "IsSortable", true);
        builder.CloseComponent();
    };


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("My Table")]
    public async Task Should_be_able_to_set_the_title_for_the_table_that_defaults_to_results_if_null_empty_or_whitespace(string? title)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.Title, title));

        var textContent = table.Find("div > div").TextContent;

        if (String.IsNullOrWhiteSpace(title))
        {
            textContent.Should().Be(GlobalValues.DataTable_Title_Text);
            return;
        }

        textContent.Should().Be(title);
    }


    [Fact]
    public async Task Should_be_able_hide_the_title()
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.Title, "My Title").Add(x => x.TitleHidden, true));

        var titleDivClassList = table.Find("div > div").ClassList;

        titleDivClassList.Should().Contain(GlobalValues.DataTable_Title_Hidden_Modifier);
    }

    [Theory]
    [InlineData(TitleAlignment.Start)]
    [InlineData(TitleAlignment.Centre)]
    [InlineData(TitleAlignment.End)]
    public async Task Should_be_able_to_set_the_title_alignment(TitleAlignment titleAlignment)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.TitleAlignment, titleAlignment));

        var titleDiv = table.Find($"div.{GlobalValues.DataTable_Title_Class}");

        var positionValue = DataTableHelper.GetTitlePosition(titleAlignment);

        titleDiv.GetAttribute("data-br-data-position").Should().Be(positionValue);

    }

    [Theory]
    [InlineData(FilterAlignment.Start)]
    [InlineData(FilterAlignment.Centre)]
    [InlineData(FilterAlignment.End)]
    public async Task Should_be_able_to_set_the_filter_alignment(FilterAlignment filterAlignment)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.FilterAlignment, filterAlignment));

        var filterDiv = table.Find($"div.{GlobalValues.DataTable_Filter_Area_Class} > div");

        var positionValue = DataTableHelper.GetFilterPosition(filterAlignment);

        filterDiv.GetAttribute("data-br-filter-position").Should().Be(positionValue);
    }

    [Fact]
    public async Task Should_be_able_to_add_content_inside_the_filter_render_fragment()
    {
        await using var context = new BunitContext();

        var filterMarkup = "<div>My Filter</div>";

        var table = CreateDataTable(context, p => p.Add(x => x.Filter, filterMarkup));

        var filterDiv = table.Find($"div.{GlobalValues.DataTable_Filter_Area_Class} > div");

        filterDiv.InnerHtml.Should().Be(filterMarkup);
    }

    [Fact]
    public async Task Should_be_able_to_add_content_inside_the_top_pager_render_fragment()
    {
        await using var context = new BunitContext();

        var pagerMarkup = "<div>Top Pager</div>";

        var table = CreateDataTable(context, p => p.Add(x => x.TopPager, pagerMarkup));

        var pagerDiv = table.Find($"div.{GlobalValues.DataTable_Top_Pager_Class}");

        pagerDiv.InnerHtml.Should().Be(pagerMarkup);
    }
    [Fact]
    public async Task Should_be_able_to_add_content_inside_the_bottom_pager_render_fragment()
    {
        await using var context = new BunitContext();

        var pagerMarkup = "<div>Bottom Pager</div>";

        var table = CreateDataTable(context, p => p.Add(x => x.BottomPager, pagerMarkup));

        var pagerDiv = table.Find($"div.{GlobalValues.DataTable_Bottom_Pager_Class}");

        pagerDiv.InnerHtml.Should().Be(pagerMarkup);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(20)]
    [InlineData(32)]//the default - will not add but should get this value
    public async Task Should_be_able_to_set_the_virtualise_item_size_that_defaults_to_32_if_not_provided(int virtualiseSize)
    {
        await using var context = new BunitContext();

        var table = virtualiseSize != 32 ? CreateDataTable(context, p => p.Add(x => x.VirtualizeItemSizePX, virtualiseSize)) : CreateDataTable(context);
        table.Instance.VirtualizeItemSizePX.Should().Be(virtualiseSize);

    }

    [Fact]
    public async Task Should_be_able_to_provide_a_pager_binding_object_for_the_pagers()
    {
        await using var context = new BunitContext();

        var pagerBinding = new PagerBinding(currentPage: 10, currentItemCount: 1000, totalItemCount: 2000, itemsPerPage: 50);

        var table = CreateDataTable(context, p => p.Add(x => x.PagerBinding, pagerBinding));

        //No data source so the OnParametersSetAsync will set everything to zero excluding the items per page
        table.Instance.PagerBinding.Should().Match<PagerBinding>(b => b.CurrentPage == 0 && b.CurrentItemCount == 0 && b.TotalItemCount == 0 && b.ItemsPerPage == 50);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Pick")]
    public async Task Should_be_able_to_set_the_select_column_header_that_defaults_to_select_if_null_empty_or_whitespace(string? rowSelectHeading)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.RowSelectHeading, rowSelectHeading).Add(x => x.RowSelectionMode, RowSelectionMode.Single));

        var selectColumnHeader = table.Find($"th.{GlobalValues.DataTable_Column_Selector_Class}");

        if (String.IsNullOrWhiteSpace(rowSelectHeading))
        {
            selectColumnHeader.TextContent.Should().Be(GlobalValues.DataTable_Row_Selector_Header_Text);
            return;
        }
        selectColumnHeader.TextContent.Should().Be(rowSelectHeading);

    }

    [Theory]
    [InlineData(RowSelectionMode.None)]
    [InlineData(RowSelectionMode.Single)]
    [InlineData(RowSelectionMode.Multiple)]
    public async Task Should_be_able_to_set_the_row_selection_mode_none_means_no_checkbox_column(RowSelectionMode rowSelectionMode)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.RowSelectionMode, rowSelectionMode));

        using (new AssertionScope())
        {
            table.Instance.RowSelectionMode.Should().Be(rowSelectionMode);

            if (rowSelectionMode != RowSelectionMode.None)
            {
                table.Find($"th.{GlobalValues.DataTable_Column_Selector_Class}").TextContent.Should().NotBeNull();
                return;
            }

            table.FindAll($"th.{GlobalValues.DataTable_Column_Selector_Class}").Count.Should().Be(0);
        }
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("No entries in the system.")]
    public async Task Should_be_able_to_set_the_text_displayed_when_there_are_no_records_to_display(string? noRecordsText)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.NoRecordText, noRecordsText));

        var lastRowContent = table.Find($"tr.{GlobalValues.DataTable_Row_No_Data_Class} > td").TextContent;

        if (String.IsNullOrWhiteSpace(noRecordsText))
        {
            lastRowContent.Should().Be(GlobalValues.DataTable_No_Records_Text);
            return;
        }

        lastRowContent.Should().Be(noRecordsText);

    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Look at all the record we have: {totalrows} row")]
    public async Task Should_be_able_to_set_the_record_count_text(string? recordCountText)
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> { new Contact(1, "Mr", "John", "Doe", new DateOnly(1970, 1, 1), "United Kingdome", 100.00m) };
        var table = CreateDataTable(context, p => p.Add(x => x.RecordCountText, recordCountText).Add(x => x.DataSource, dataSource));

        var rowCountDisplay = table.Find($"div.{GlobalValues.DataTable_Messages_Class}").TextContent;

        if (String.IsNullOrWhiteSpace(recordCountText))
        {
            rowCountDisplay.Should().Be("Showing 1 items.");
            return;
        }

        rowCountDisplay.Should().Be("Look at all the record we have: 1 row");

    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("You have {filteredrows} rows from the total of {totalrows} rows")]
    public async Task Should_be_able_to_set_the_filtered_record_count_text(string? filterCountText)
    {
        await using var context = new BunitContext();

        Func<Contact, bool> filter = (c) => c.GivenName == "John";

        var dataSource = new List<Contact> {
                    new Contact(1, "Mr", "John", "Doe", new DateOnly(1970, 1, 1), "United Kingdome", 100.00m),
                    new Contact(2, "Mrs", "Jane", "Doe", new DateOnly(1970, 1, 1), "United Kingdome", 100.00m)
                };

        var table = CreateDataTable(context, p => p.Add(x => x.FilterCountText, filterCountText).Add(x => x.DataSource, dataSource).Add(x => x.FilterRule, filter));

        var expectedText = String.IsNullOrWhiteSpace(filterCountText) ? "Showing 1 filtered items from a total of 2." : "You have 1 rows from the total of 2 rows";


        table.WaitForAssertion(() =>
            table.Find($"div.{GlobalValues.DataTable_Messages_Class}").TextContent.Should().Be(expectedText));
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Press to activate.")]
    public async Task Should_be_able_to_set_the_hidden_aria_description_for_sorting_to_let_users_know_what_the_button_does(string? pressToSortText)
    {
        await using var context = new BunitContext();

        var table = CreateDataTable(context, p => p.Add(x => x.PressToSortText, pressToSortText));

        if (String.IsNullOrWhiteSpace(pressToSortText))
        {
            table.Find("span[hidden]").TextContent.Should().Be(GlobalValues.DataTable_Press_To_Sort_Text);
            return;
        }

        table.Find("span[hidden]").TextContent.Should().Be(pressToSortText);
    }
}
