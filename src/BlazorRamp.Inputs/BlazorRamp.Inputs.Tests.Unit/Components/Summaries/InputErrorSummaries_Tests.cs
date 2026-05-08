using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Components;
using BlazorRamp.Inputs.Components.Summaries;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BlazorRamp.Inputs.Tests.Unit.Components.Summaries;

public class InputErrorSummaries_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2 and 50 characters.")]
        public string Name { get; set; } = null!;

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
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Input_Focus, _ => true).SetVoidResult();

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



        [Fact]
        public async Task Should_capture_unmatched_attributes_for_the_summary_section()
        {
            await using var context = new BunitContext();

            var (summary, editContext) = CreateSummaryWithTextInput(context,p => p.AddUnmatched("style", "color:red;"));

            summary.Find("section").GetAttribute("style").Should().Be("color:red;");
        }

        [Theory]
        [InlineData(TitleHeadingLevel.H2)]
        [InlineData(TitleHeadingLevel.H3)]
        [InlineData(TitleHeadingLevel.H4)]
        [InlineData(TitleHeadingLevel.H5)]
        [InlineData(TitleHeadingLevel.H6)]
        [InlineData((TitleHeadingLevel)99)]
        public async Task Should_be_able_to_set_the_heading_level_that_defaults_to_h3(TitleHeadingLevel headingLevel)
        {
            await using var context = new BunitContext();

            var (summary, editContext) = CreateSummaryWithTextInput(context, p => p.Add(p => p.TitleHeadingLevel, headingLevel));

            if (headingLevel == (TitleHeadingLevel)99)
            {
                summary.Find("section").FirstChild!.NodeName.Should().Be("H3");
                return;
            }

            summary.Find("section").FirstChild!.NodeName.Should().Be(headingLevel.ToString().ToUpper());
        }

    }


    [Theory]
    [InlineData(SummaryDisplay.OnModelValidated)]
    [InlineData(SummaryDisplay.Always)]
    public async Task Should_be_able_to_set_the_summary_display(SummaryDisplay summaryDisplay)
    {

        await using var context = new BunitContext();

        var (summary, editContext) = CreateSummaryWithTextInput(context, p => p.Add(p => p.SummaryDisplay, summaryDisplay));

        summary.Instance.SummaryDisplay.ToString().Should().Be(summaryDisplay.ToString());
    }


    [Fact]
    public async Task Summary_display_should_default_to_on_model_validated()
    {
        await using var context = new BunitContext();

        var (summary, editContext) = CreateSummaryWithTextInput(context, p => p.Add(p => p.Title, "No summary display set"));

        summary.Instance.SummaryDisplay.Should().Be(SummaryDisplay.OnModelValidated);
    }


    [Fact]
    public async Task Should_be_able_to_set_the_input_suffix_that_gets_appended_to_the_label_text()
    {

        await using var context = new BunitContext();

        var (summary, editContext) = CreateSummaryWithTextInput(context, p => p.Add(p => p.InputSuffix,"Field Suffix"));

        var input  = summary.Find("input");

        await summary.InvokeAsync(() => editContext.Validate());

        summary.WaitForAssertion(() =>
        {
            summary.Find("a").TextContent.Should().Be("Name Field Suffix");
        });
    }
}
