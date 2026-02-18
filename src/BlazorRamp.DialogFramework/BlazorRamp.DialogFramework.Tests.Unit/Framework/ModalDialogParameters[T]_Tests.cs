using AngleSharp.Common;
using BlazorRamp.DialogFramework.Framework;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class ModalDialogParameters_Tests
{
    [Fact]
    public void Add_should_add_a_data_parameter_to_its_internal_collection()
    {
        var dialogParameters = new ModalDialogParameters<FakeDialogComponent>();

        dialogParameters.Add<string>(d => d.Title, "Dialog Title");

        using(new AssertionScope())
        {
            dialogParameters.Count().Should().Be(1);

            dialogParameters.GetItemByIndex(0).Value.Should().Be("Dialog Title");

            dialogParameters.GetItemByIndex(0).Value.GetType().Should().Be(typeof(string));
        }

    }

    [Fact]
    public void Multiple_adds_should_each_add_a_data_parameter_to_its_internal_collection()
    {
        var dialogParameters = new ModalDialogParameters<FakeDialogComponent>();

        dialogParameters.Add<string>(d => d.Title, "Dialog Title");
        dialogParameters.Add<int>(d => d.Count, 42);

        using (new AssertionScope())
        {
            dialogParameters.Count().Should().Be(2);

            dialogParameters.GetItemByIndex(1).Value.Should().Be(42);

            dialogParameters.GetItemByIndex(1).Value.GetType().Should().Be(typeof(int));
        }

    }

    [Fact]
    public void Adding_a_param_with_mismatched_data_types_should_throw_an_argument_exeption()
    {
        var dialogParameters = new ModalDialogParameters<FakeDialogComponent>();

        FluentActions.Invoking(() => dialogParameters.Add<int>(d => d.Title, 42)).Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void Internal_collection_should_be_enumerable()
    {
        var dialogParameters = new ModalDialogParameters<FakeDialogComponent>();

        dialogParameters.Add<string>(d => d.Title, "Dialog Title");
        dialogParameters.Add<int>(d => d.Count, 42);

        int parameterCount = 0;
        foreach(var parameter in dialogParameters)
        {
            parameterCount++;
        }

        parameterCount.Should().Be(2);
    }
    [Fact]
    public void The_non_generic_get_enumerator_should_enumerate_the_collection()
    {
        var dialogParameters = new ModalDialogParameters<FakeDialogComponent>();
        dialogParameters.Add<string>(d => d.Title, "Dialog Title");
        dialogParameters.Add<int>(d => d.Count, 42);

        var nonGeneric = (IEnumerable)dialogParameters;
        int parameterCount = 0;
        foreach (var item in nonGeneric)
        {
            parameterCount++;
        }

        parameterCount.Should().Be(2);
    }
}
