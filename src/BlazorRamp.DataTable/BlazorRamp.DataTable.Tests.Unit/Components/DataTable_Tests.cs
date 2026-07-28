using BlazorRamp.DataTable.Tests.Unit.Shared;
using Bunit;
using BlazorRamp.DataTable.Components;
using Microsoft.Extensions.DependencyInjection;
using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class DataTable_Tests
{

    public static IRenderedComponent<DataTable<Contact>> CreateDataTable(BunitContext context,
        Action<ComponentParameterCollectionBuilder<BlazorRamp.DataTable.Components.DataTable<Contact>>>? parameters = null)
    {
        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var component = context.Render<DataTable<Contact>>(builder =>
        {
            builder.Add(x => x.TableColumns, DefaultTableColumns);
            parameters?.Invoke(builder);
        });

        return component;
    }

    public static RenderFragment DefaultTableColumns => builder =>
    {
        builder.OpenComponent<DataColumn<Contact>>(0);
        builder.AddAttribute(1, "DataProperty", (Func<Contact, object>)(d => d.GivenName));
        builder.AddAttribute(2, "DisplayName", "First name");
        builder.AddAttribute(3, "IsSortable", true);
        builder.CloseComponent();

        builder.OpenComponent<DataColumn<Contact>>(4);
        builder.AddAttribute(5, "DataProperty", (Func<Contact, object>)(d => d.Country));
        builder.AddAttribute(6, "DisplayName", "Country");
        builder.AddAttribute(7, "IsSortable", false);
        builder.CloseComponent();
    };

}
