using BlazorRamp.DialogFramework.Common.Constants;
using BlazorRamp.DialogFramework.Framework;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Data;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework
{
    public class ModalDialogResult_Tests
    {
        [Fact]
        public void The_static_OK_factory_method_the_without_the_data_param_should_return_a_contructed_modal_dialog_result()
        {
            var dialogResult = ModalDialogResult.OK();

            dialogResult.Should().Match<ModalDialogResult>(d => d.Data == (object)NoReturnValue.Value && d.DataType == typeof(NoReturnValue)
                                                         && d.ButtonText == "OK" && d.ButtonClicked == DialogResultButtons.Ok);
        }

        [Fact]
        public void The_static_OK_factory_method_with_the_data_param_should_return_a_contructed_modal_dialog_result()
        {
            var personData   = StaticData.ConstructedPersonData();
            var dialogResult = ModalDialogResult.OK(personData);

            dialogResult.Should().Match<ModalDialogResult>(d => d.Data == (object)personData && d.DataType == typeof(SomePersonData)
                                                         && d.ButtonText == "OK" && d.ButtonClicked == DialogResultButtons.Ok);
        }

        [Fact]
        public void The_static_cancel_factory_method_the_without_the_data_param_should_return_a_contructed_cancelled_modal_dialog_result()
        {
            var dialogResult = ModalDialogResult.Cancel();

            dialogResult.Should().Match<ModalDialogResult>(d => d.Data == (object)NoReturnValue.Value && d.DataType == typeof(NoReturnValue)
                                                         && d.ButtonText == "Cancelled" && d.ButtonClicked == DialogResultButtons.Cancel);
        }


        [Fact]
        public void The_static_other_factory_method_the_without_the_data_param_should_return_a_contructed_cancelled_modal_dialog_result()
        {
            var buttonText = "Other Result";
            var dialogResult = ModalDialogResult.Other(buttonText);

            dialogResult.Should().Match<ModalDialogResult>(d => d.Data == (object)NoReturnValue.Value && d.DataType == typeof(NoReturnValue)
                                                         && d.ButtonText == buttonText && d.ButtonClicked == DialogResultButtons.Other);
        }

        [Fact]
        public void The_static_other_factory_method_with_the_data_param_should_return_a_contructed_cancelled_modal_dialog_result()
        {
            var personData   = StaticData.ConstructedPersonData();
            var buttonText   = "Other Result";
            var dialogResult = ModalDialogResult.Other(buttonText, personData);

            dialogResult.Should().Match<ModalDialogResult>(d => d.Data == (object)personData && d.DataType == typeof(SomePersonData)
                                                         && d.ButtonText == buttonText && d.ButtonClicked == DialogResultButtons.Other);
        }
    }
}
