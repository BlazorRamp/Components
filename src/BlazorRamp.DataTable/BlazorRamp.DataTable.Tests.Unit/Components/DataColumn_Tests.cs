using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Utilities;
using BlazorRamp.DataTable.Components;
using BlazorRamp.DataTable.Tests.Unit.SharedTestModels;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class DataColumn_Tests
{
    public static IRenderedComponent<DataTable<Contact>> CreateDataTableWithColumn(BunitContext context,
    Action<ComponentParameterCollectionBuilder<DataColumn<Contact>>> columnParameters,
    Action<ComponentParameterCollectionBuilder<DataTable<Contact>>>? parameters = null)
    {
        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var busyModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.BusyIndicator/assets/js/busy-indicator.js");
        var coreModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.Core/assets/js/core-live-region.js");

        busyModuleInterop.SetupVoid();
        coreModuleInterop.SetupVoid();
        coreModuleInterop.SetupVoid("announcement", _ => true).SetVoidResult();

        return context.Render<DataTable<Contact>>(builder =>
        {
            builder.Add(x => x.TableColumns, columnParameters);
            parameters?.Invoke(builder);
        });
    }

    public class Parameters
    {
        [Fact]
        public async Task Should_render_the_display_name_as_the_column_header()
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var table = CreateDataTableWithColumn(context,
                c => c.Add(x => x.DataProperty, (Expression<Func<Contact, object>>)(d => d.GivenName))
                      .Add(x => x.DisplayName, "First name"),
                p => p.Add(x => x.DataSource, dataSource));

            table.Find("th span").TextContent.Should().Be("First name");
        }

        [Theory]
        [InlineData(ColumnAlignment.Start)]
        [InlineData(ColumnAlignment.Centre)]
        [InlineData(ColumnAlignment.End)]
        public async Task Should_be_able_to_set_the_column_alignment(ColumnAlignment columnAlignment)
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var table = CreateDataTableWithColumn(context,
                                    c => c.Add(x => x.DataProperty, (Expression<Func<Contact, object>>)(d => d.GivenName))
                                          .Add(x => x.DisplayName, "First name")
                                          .Add(x => x.ColumnAlignment, columnAlignment),
                                    p => p.Add(x => x.DataSource, dataSource));

            var positionValue = DataTableHelper.GetDataPosition(columnAlignment);

            table.Find("th").GetAttribute("data-br-data-position").Should().Be(positionValue);

        }

        [Fact]
        public async Task Should_be_able_to_provide_style_content_for_the_header_style_attribute()
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var styleContent = "background-color:red;color:blue;";

            var table = CreateDataTableWithColumn(context,
                                    c => c.Add(x => x.DataProperty, (Expression<Func<Contact, object>>)(d => d.GivenName))
                                          .Add(x => x.DisplayName, "First name")
                                          .Add(x => x.HeaderStyle,styleContent),
                                    p => p.Add(x => x.DataSource, dataSource));

        
            table.Find("th").GetAttribute("style").Should().Be(styleContent);
        
        }
        [Fact]
        public async Task Should_be_able_to_provide_style_content_for_the_cell_style_attribute()
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var styleContent = "background-color:red;color:blue;";

            var table = CreateDataTableWithColumn(context,
                                    c => c.Add(x => x.DataProperty, (Expression<Func<Contact, object>>)(d => d.GivenName))
                                          .Add(x => x.DisplayName, "First name")
                                          .Add(x => x.CellStyle, styleContent),
                                    p => p.Add(x => x.DataSource, dataSource));


            table.Find("tbody > tr > td").GetAttribute("style").Should().Be(styleContent);

        }

        [Fact]
        public async Task Should_be_able_to_provide_a_cell_format_string_for_formattable_content()
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var table = CreateDataTableWithColumn(context,
                                    c => c.Add(x => x.DataProperty, (Expression<Func<Contact, object>>)(d => d.DateOfBirth))
                                          .Add(x => x.DisplayName, "DOB")
                                          .Add(x => x.CellFormat, "yyyy-MM-dd"),
                                    p => p.Add(x => x.DataSource, dataSource));

            table.Find("tbody > tr > td").TextContent.Should().StartWith("1970-01-01");
        }

        [Fact]
        public async Task Should_be_able_to_provide_a_cell_template_for_the_cells_content()
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var table = CreateDataTableWithColumn(context,
                                    c => c.Add(x => x.DataProperty, (Expression<Func<Contact, object>>)(d => d.GivenName))
                                          .Add(x => x.DisplayName, "First name")
                                         .Add(x => x.CellTemplate, contact => $"<strong>{contact.GivenName}</strong>"),
                                    p => p.Add(x => x.DataSource, dataSource));

            table.Find("tbody > tr > td").InnerHtml.Should().Be("<strong>Anne</strong>");

        }

    }
}
