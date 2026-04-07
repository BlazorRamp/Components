using BlazorRamp.Inputs.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using PasswordInputComponent = BlazorRamp.Inputs.Components.PasswordInput;

namespace BlazorRamp.Inputs.Tests.Unit.Components;

public class PasswordInput_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Needs a value")]
        public string PropertyValue { get; set; } = "test";
    }

    public static (IRenderedComponent<PasswordInputComponent> Component, EditContext EditContext) CreateInputWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);

        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<PasswordInputComponent>(
            builder => builder
                .AddCascadingValue(editContext)
                .Add(p => p.Value, model.PropertyValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                .Add(p => p.ValueExpression, () => model.PropertyValue)
                .TryAdd(paramName, paramValue));

        return (component, editContext);
    }

    public class Parameters()
    {

    }

}
