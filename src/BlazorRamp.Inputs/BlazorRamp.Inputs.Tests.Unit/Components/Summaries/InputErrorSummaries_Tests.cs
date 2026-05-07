using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Components;
using BlazorRamp.Inputs.Components.Summaries;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Inputs.Tests.Unit.Components.Summaries;

public class InputErrorSummaries_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required.")]
        [Range(18, 99, ErrorMessage = "Must be between 18 and 99.")]
        public int Age { get; set; } = 0;
    }

    private static (IRenderedComponent<InputErrorsSummary> Summary, EditContext EditContext) CreateSummaryWithTextInput(
        BunitContext context,
        Action<ComponentParameterCollectionBuilder<InputErrorsSummary>>? summaryParams = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Summary_Focus, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);
        editContext.EnableDataAnnotationsValidation(context.Services);

        var summary = context.Render<InputErrorsSummary>(builder =>
        {
            builder
                .AddCascadingValue(editContext)
                .Add(p => p.ChildContent, childBuilder =>
                {
                    childBuilder.OpenComponent<TextInput>(0);
                    childBuilder.AddAttribute(1, nameof(TextInput.Value), model.Name);
                    childBuilder.AddAttribute(2, nameof(TextInput.ValueChanged), EventCallback.Factory.Create<string>(context, v => model.Name = v));
                    childBuilder.AddAttribute(3, nameof(TextInput.ValueExpression), (Expression<Func<string>>)(() => model.Name));
                    childBuilder.AddAttribute(4, nameof(TextInput.LabelText), "Name");
                    childBuilder.CloseComponent();
                });

            summaryParams?.Invoke(builder);
        });

        return (summary, editContext);
    }
    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Please fix the following")]
        public async Task Should_use_default_title_when_null_empty_or_whitespace_otherwise_use_supplied_title(string? title)
        {
            await using var context = new BunitContext();

            var (summary, editContext) = CreateSummaryWithTextInput(context,p => p.Add(x => x.Title, title));

            await summary.InvokeAsync(() => editContext.Validate());

            summary.WaitForAssertion(() =>
            {
                var heading = summary.Find($".{GlobalValues.Input_Errors_Summary_Heading}");

                if (String.IsNullOrWhiteSpace(title))
                {
                    heading.TextContent.Should().Be(GlobalValues.Input_Errors_Summary_Title);
                }
                else
                {
                    heading.TextContent.Should().Be(title);
                }
            });
        }

    }
}
