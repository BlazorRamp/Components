using BlazorRamp.Core.Common.Extensions;
using BlazorRamp.Core.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRamp.Core.Tests.Unit.Common.Extensions;

public class ServiceCollectionExtensions_Tests
{
    public class AddBlazorRampCore
    {
        [Fact]
        public void Should_register_ILiveRegionService_as_scoped_and_return_service_collection()
        {
            var services = new ServiceCollection();
            var result   = services.AddBlazorRampCore();

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeSameAs(services, "because the method should return the same service collection instance for fluent API chaining");

                var serviceDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(ILiveRegionService));

                using (new AssertionScope())
                {
                    serviceDescriptor.Should().NotBeNull("because ILiveRegionService should be registered");
                    serviceDescriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped, "because the service should be scoped");
                    serviceDescriptor.ImplementationType.Should().Be(typeof(LiveRegionService), "because LiveRegionService should be the implementation");
                }
            }
        }
    }
}
