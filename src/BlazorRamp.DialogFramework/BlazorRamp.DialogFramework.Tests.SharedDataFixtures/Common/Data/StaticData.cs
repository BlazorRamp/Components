using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;

namespace BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Data;

public static class StaticData
{
    public static SomePersonData ConstructedPersonData()

        => new SomePersonData("John", "Doe", 42, "UK");  
}
