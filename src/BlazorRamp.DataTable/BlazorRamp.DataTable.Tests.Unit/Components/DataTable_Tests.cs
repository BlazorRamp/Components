using BlazorRamp.DataTable.Tests.Unit.Shared;
using Bunit;
using BlazorRamp.DataTable.Components;
using Microsoft.Extensions.DependencyInjection;
using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;
using FluentAssertions;
using BlazorRamp.DataTable.Common.Constants;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class DataTable_Tests
{

    public static IRenderedComponent<DataTable<Contact>> CreateDataTable(BunitContext context,
        Action<ComponentParameterCollectionBuilder<DataTable<Contact>>>? parameters = null)
    {
        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var busyModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.BusyIndicator/assets/js/busy-indicator.js");
        var coreModuleInterop = context.JSInterop.SetupModule("./_content/BlazorRamp.Core/assets/js/core-live-region.js");

        busyModuleInterop.SetupVoid();
        coreModuleInterop.SetupVoid();

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
        Expression<Func<Contact, object>> country = c => c.Country;

        builder.OpenComponent<DataColumn<Contact>>(0);
        builder.AddAttribute(1, "DataProperty",givenName);
        builder.AddAttribute(2, "DisplayName", "First name");
        builder.AddAttribute(3, "IsSortable", true);
        builder.CloseComponent();

        builder.OpenComponent<DataColumn<Contact>>(4);
        builder.AddAttribute(5, "DataProperty", country);
        builder.AddAttribute(6, "DisplayName", "Country");
        builder.AddAttribute(7, "IsSortable", false);
        builder.CloseComponent();
    };

    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("My Table")]
        public async Task Should_be_able_to_set_the_title_for_the_table_that_defaults_to_results_if_null_empty_or_whitespace(string? title)
        {
            await using var context = new BunitContext();

            var table = CreateDataTable(context,p => p.Add(x => x.Title, title));

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
    }


}
