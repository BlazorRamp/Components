using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Components;
using BlazorRamp.DataTable.Tests.Unit.SharedTestModels;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DataTable.Tests.Unit.Components;

public class TemplateColumn_Tests
{
    public static IRenderedComponent<DataTable<Contact>> CreateDataTableWithColumn(BunitContext context,
                  Action<ComponentParameterCollectionBuilder<TemplateColumn<Contact>>> columnParameters,
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

        return context.Render<DataTable<Contact>>(builder =>
        {
            builder.Add(x => x.TableColumns, columnParameters);
            parameters?.Invoke(builder);
        });
    }

    public class Parameters
    {

        [Fact]
        public async Task Should_be_able_to_set_a_templated_header()
        {
            await using var context = new BunitContext();

            var dataSource = new List<Contact> { new Contact(1, "Mrs", "Anne", "Doe", new DateOnly(1970, 1, 1), "UK", 1m) };

            var table = CreateDataTableWithColumn(context,
                c => c.Add(x => x.HeaderTemplate, c => "<span>Custom Header</span>")
                .Add(x => x.CellTemplate, contact => $"<strong>{contact.GivenName}-{contact.FamilyName}</strong>"),
                p => p.Add(x => x.DataSource, dataSource));

            using (new AssertionScope())
            {
                table.Find("th").InnerHtml.Should().Be("<span>Custom Header</span>");
                table.Find("td").InnerHtml.Should().Be("<strong>Anne-Doe</strong>");
            }
        }
    }
}
