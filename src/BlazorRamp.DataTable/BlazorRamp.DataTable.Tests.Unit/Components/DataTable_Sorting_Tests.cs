using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Components;
using BlazorRamp.DataTable.Tests.Unit.SharedTestModels;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class DataTable_Sorting_Tests
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
    public async Task Should_sort_ascending_then_descending_then_unsorted_when_header_clicked_repeatedly()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(3, "Mr", "Cal", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource));

        var sortButton = table.Find("button." + GlobalValues.DataTable_Column_Sorter_Class);
        using (new AssertionScope())
        {
            await sortButton.ClickAsync();
            table.FindAll("tbody td").First().TextContent.Should().Be("Anne");

            await sortButton.ClickAsync();
            table.FindAll("tbody td").First().TextContent.Should().Be("Cal");

            await sortButton.ClickAsync();
            table.FindAll("tbody td").First().TextContent.Should().Be("Bob");
        }
    }

    [Fact]
    public async Task Should_sort_non_string_columns_such_as_numbers()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
        new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 11.36m),
        new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1.25m),
        new Contact(3, "Mr", "Cal", "Doe", new DateOnly(1970,1,1), "UK", 32.65m)
    };

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource));

        using (new AssertionScope())
        {
            await table.FindAll("button." + GlobalValues.DataTable_Column_Sorter_Class)[1].ClickAsync();
            table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelectorAll("td")[1].TextContent.Should().Be("1.25");

            await table.FindAll("button." + GlobalValues.DataTable_Column_Sorter_Class)[1].ClickAsync();
            table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelectorAll("td")[1].TextContent.Should().Be("32.65");

            await table.FindAll("button." + GlobalValues.DataTable_Column_Sorter_Class)[1].ClickAsync();
            table.FindAll($"tbody tr.{GlobalValues.DataTable_Data_Row_Class}")[0].QuerySelectorAll("td")[1].TextContent.Should().Be("11.36");
        }
    }

    [Fact]
    public async Task Should_set_aria_sort_on_the_header_cell_reflecting_current_sort_state()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource));

        var headerCell = table.FindAll("th").First(); // GivenName column, sortable
        var sortButton = table.Find("button." + GlobalValues.DataTable_Column_Sorter_Class);

        // Not sorted initially — attribute should not be present
        headerCell.HasAttribute("aria-sort").Should().BeFalse();

        using (new AssertionScope())
        {

            await sortButton.ClickAsync();
            table.WaitForAssertion(() => table.FindAll("th").First().GetAttribute("aria-sort").Should().Be("ascending"));

            await sortButton.ClickAsync();
            table.WaitForAssertion(() => table.FindAll("th").First().GetAttribute("aria-sort").Should().Be("descending"));

            await sortButton.ClickAsync();
            table.WaitForAssertion(() => table.FindAll("th").First().GetAttribute("aria-sort").Should().BeNull());
        }
    }

    [Fact]
    public async Task Should_sort_by_the_default_sort_index_column_on_first_render()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(3, "Mr", "Cal", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource).Add(x => x.DefaultSortIndex, 0));

        table.WaitForAssertion(() => table.FindAll("tbody td").First().TextContent.Should().Be("Anne"));
    }


    [Fact]
    public async Task Should_not_sort_when_default_sort_index_is_negative_one()
    {
        await using var context = new BunitContext();

        var dataSource = new List<Contact> {
                new Contact(2, "Mr", "Bob", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970,1,1), "UK", 1m),
                new Contact(3, "Mr", "Cal", "Doe", new DateOnly(1970,1,1), "UK", 1m)
            };

        var table = CreateDataTable(context, p => p.Add(x => x.DataSource, dataSource)); // DefaultSortIndex defaults to -1

        table.FindAll("tbody td").First().TextContent.Should().Be("Bob"); // original order, unsorted
    }

}
