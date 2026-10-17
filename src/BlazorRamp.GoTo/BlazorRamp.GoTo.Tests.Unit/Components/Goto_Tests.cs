using BlazorRamp.GoTo.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using GoToComponent = BlazorRamp.GoTo.Components.GoTo;

namespace BlazorRamp.GoTo.Tests.Unit.Components;

public class GoTo_Tests
{
    public const string _labelText = "Some label text";

    public static IRenderedComponent<GoToComponent> CreateGoTo(BunitContext context, Action<ComponentParameterCollectionBuilder<GoToComponent>>? parameters = null)
    {
        // The marker injects no services and calls no JavaScript, so there is nothing to register or fake.
        var component = context.Render<GoToComponent>(
            builder =>
            {
                parameters?.Invoke(builder);
            });

        return component;
    }


    public class Parameters
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_throw_null_exception_when_label_text_is_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            FluentActions.Invoking(() => CreateGoTo(context, p => p.Add(x => x.LabelText, labelText)))
                         .Should().ThrowExactly<ArgumentNullException>()
                         .WithParameterName("LabelText")
                         .WithMessage($"*{GlobalValues.GoTo_Label_Text_Exception_Message}*");
        }

        [Fact]
        public async Task Should_be_able_to_set_the_label_text()
        {
            await using var context = new BunitContext();

            var component = CreateGoTo(context, p => p.Add(x => x.LabelText, _labelText));

            using (new AssertionScope())
            {
                // The markup puts the text on its own line, so the raw text content includes whitespace.
                component.Find($".{GlobalValues.GoTo_Label_Class} > span").TextContent.Trim().Should().Be(_labelText);
                component.Instance.LabelText.Should().Be(_labelText);
            }
        }

        [Fact]
        public async Task Should_trim_the_label_text()
        {
            await using var context = new BunitContext();

            var padded = CreateGoTo(context, p => p.Add(x => x.LabelText, $"  {_labelText}  "));
            var trimmed = CreateGoTo(context, p => p.Add(x => x.LabelText, _labelText));

            // The surrounding template whitespace is identical, so the two only match if the padding was removed.
            padded.Find($".{GlobalValues.GoTo_Label_Class} > span").TextContent
                  .Should().Be(trimmed.Find($".{GlobalValues.GoTo_Label_Class} > span").TextContent);
        }

        [Fact]
        public async Task Should_update_the_label_text_when_the_value_changes()
        {
            await using var context = new BunitContext();

            var component = CreateGoTo(context, p => p.Add(x => x.LabelText, "First label"));

            component.Render(p => p.Add(x => x.LabelText, "Second label"));

            component.Find($".{GlobalValues.GoTo_Label_Class} > span").TextContent.Trim().Should().Be("Second label");
        }
    }


    public class Markup
    {
        [Fact]
        public async Task Should_render_the_attribute_the_viewer_uses_to_find_markers()
        {
            await using var context = new BunitContext();

            var component = CreateGoTo(context, p => p.Add(x => x.LabelText, _labelText));

            component.Find($".{GlobalValues.GoTo_Block_Class}").HasAttribute("data-br-goto").Should().BeTrue();
        }

        [Fact]
        public async Task Should_render_the_label_as_a_popover_dialog_named_by_its_text()
        {
            await using var context = new BunitContext();

            var component = CreateGoTo(context, p => p.Add(x => x.LabelText, _labelText));

            var label = component.Find($".{GlobalValues.GoTo_Label_Class}");
            var text = component.Find($".{GlobalValues.GoTo_Label_Class} > span");

            using (new AssertionScope())
            {
                label.LocalName.Should().Be("dialog");
                label.GetAttribute("popover").Should().Be("auto");
                label.GetAttribute("aria-labelledby").Should().Be(text.Id);
            }
        }

        [Fact]
        public async Task Should_give_each_marker_its_own_ids()
        {
            await using var context = new BunitContext();

            var first = CreateGoTo(context, p => p.Add(x => x.LabelText, "First label"));
            var second = CreateGoTo(context, p => p.Add(x => x.LabelText, "Second label"));

            var ids = first.FindAll("[id]").Concat(second.FindAll("[id]")).Select(x => x.Id).ToList();

            ids.Should().HaveCount(6).And.OnlyHaveUniqueItems();
        }
    }
}