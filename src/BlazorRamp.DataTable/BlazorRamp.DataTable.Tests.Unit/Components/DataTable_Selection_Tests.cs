using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Components;
using BlazorRamp.DataTable.Tests.Unit.SharedTestModels;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class DataTable_Selection_Tests
{
    public static IRenderedComponent<DataTable<Contact>> CreateDataTable(BunitContext context,
        Action<ComponentParameterCollectionBuilder<DataTable<Contact>>>? parameters = null)
    {
        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var busyModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.BusyIndicator/assets/js/busy-indicator.js");
        var coreModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.Core/assets/js/core-live-region.js");

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



    [Fact]
    public async Task Should_invoke_selected_rows_changed_with_the_clicked_row_when_a_row_is_clicked_in_single_mode()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        List<Contact>? capturedSelection = null;

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)
                                                    .Add(x => x.RowSelectionMode, RowSelectionMode.Single)
                                                    .Add(x => x.SelectedRowsChanged, EventCallback.Factory.Create<List<Contact>>(context, v => capturedSelection = v)));

        var tableRow = table.Find("tbody tr td");

        await tableRow.ClickAsync();

        capturedSelection.Should().BeEquivalentTo(new List<Contact> { dataSource[0] });
    }

    [Fact]
    public async Task Should_replace_the_selection_not_add_to_it_when_a_second_row_is_clicked_in_single_mode()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        List<Contact>? capturedSelection = null;

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)
                                                    .Add(x => x.RowSelectionMode, RowSelectionMode.Single)
                                                    .Add(x => x.SelectedRowsChanged, EventCallback.Factory.Create<List<Contact>>(context, v => capturedSelection = v)));

        await table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelector("td")!.ClickAsync();
        await table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[1].QuerySelector("td")!.ClickAsync();

        capturedSelection.Should().BeEquivalentTo(new List<Contact> { dataSource[1] });
    }

    [Fact]
    public async Task Should_accumulate_selected_rows_when_multiple_rows_are_clicked_in_multiple_mode()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        List<Contact>? capturedSelection = null;

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)
                                                    .Add(x => x.RowSelectionMode, RowSelectionMode.Multiple)
                                                    .Add(x => x.SelectedRowsChanged, EventCallback.Factory.Create<List<Contact>>(context, v => capturedSelection = v)));

        await table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelector("td")!.ClickAsync();
        await table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[1].QuerySelector("td")!.ClickAsync();

        capturedSelection.Should().BeEquivalentTo(dataSource);
    }

    [Fact]
    public async Task Should_deselect_a_row_when_clicked_a_second_time_in_multiple_mode()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        List<Contact>? capturedSelection = null;

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)
                                                    .Add(x => x.RowSelectionMode, RowSelectionMode.Multiple)
                                                    .Add(x => x.SelectedRowsChanged, EventCallback.Factory.Create<List<Contact>>(context, v => capturedSelection = v)));

        await table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelector("td")!.ClickAsync();
        await table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelector("td")!.ClickAsync();

        capturedSelection.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_toggle_selection_via_the_checkbox_input_not_just_the_row_click()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        List<Contact>? capturedSelection = null;

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)
                                                    .Add(x => x.RowSelectionMode, RowSelectionMode.Single)
                                                    .Add(x => x.SelectedRowsChanged, EventCallback.Factory.Create<List<Contact>>(context, v => capturedSelection = v)));

        var checkbox = table.Find($"td.{GlobalValues.DataTable_Row_Selector_Class} input");
        checkbox.Change(true);

        capturedSelection.Should().BeEquivalentTo(new List<Contact> { dataSource[0] });
    }

    [Fact]
    public async Task Should_only_toggle_selection_once_when_the_checkbox_is_clicked_not_twice_from_row_click_bubbling()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

        var callCount = 0;

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)
                                                    .Add(x => x.RowSelectionMode, RowSelectionMode.Single)
                                                    .Add(x => x.SelectedRowsChanged, EventCallback.Factory.Create<List<Contact>>(context, v => callCount++)));

        var checkbox = table.Find($"td.{GlobalValues.DataTable_Row_Selector_Class} input");
        checkbox.Change(true);

        callCount.Should().Be(1);
    }

}
