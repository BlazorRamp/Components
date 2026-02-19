using BlazorRamp.DialogFramework.Common.Constants;
using BlazorRamp.DialogFramework.Framework;
using Bunit;
using FluentAssertions;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework
{
    public class ModalDialogOptions_Tests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void The_constructor_should_set_all_properties_or_use_the_defaults_if_not_provided(bool hasValues)
        {
            if (hasValues)
            {

                var modalDialogOptions = new ModalDialogOptions(HorizontalAlignment.Left, VerticalAlignment.Bottom, 50);

                modalDialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "start" && o.VerticalPosition == "end" && o.MaxWidth == "50%");

                return;
            }

            _ = new ModalDialogOptions().Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center" && o.MaxWidth == "70%");

        }

        [Fact]
        public void A_width_over_100_should_be_capped_at_to_100_percent()
        {
            var modalDialogOptions = new ModalDialogOptions(HorizontalAlignment.Right, VerticalAlignment.Top, 101);

            modalDialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "end" && o.VerticalPosition == "start" && o.MaxWidth == "100%");

        }
        [Fact]
        public void A_width_less_than_15_should_be_set_to_15_percent()
        {
            var modalDialogOptions = new ModalDialogOptions(HorizontalAlignment.Right, VerticalAlignment.Top, 14);

            modalDialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "end" && o.VerticalPosition == "start" && o.MaxWidth == "15%");

        }
        [Fact]
        public void Invalid_horizontal_or_vertical_values_should_default_to_center()
        {
            var modalDialogOptions = new ModalDialogOptions((HorizontalAlignment) 99, (VerticalAlignment) 99, 50);

            modalDialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center" && o.MaxWidth == "50%");
        }

        //TODO Not sure if there is any point altering the options class to check for width below 0
    }
    
}
