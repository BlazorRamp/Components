using BlazorRamp.DialogFramework.Common.Extensions;
using BlazorRamp.DialogFramework.Framework;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.DialogFramework.Tests.Unit.Common.Extensions;


public class ServiceCollectionExtensions_Tests
{
    [Fact]
    public void Add_blazor_ramp_dialog_service_Should_register_modal_dialog_service_as_scoped()
    {
        var services = new ServiceCollection();

        services.AddBlazorRampDialogService();

        var serviceDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(ModalDialogService));
        
        using(new AssertionScope())
        {
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        }
    }
}
