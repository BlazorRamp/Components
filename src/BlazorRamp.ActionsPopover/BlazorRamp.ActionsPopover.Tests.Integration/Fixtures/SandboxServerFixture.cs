using Microsoft.AspNetCore.Mvc.Testing;
namespace BlazorRamp.ActionsPopover.Tests.Integration.Fixtures;

public class SandboxServerFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public string BaseUrl { get; private set; } = string.Empty;

    public ValueTask InitializeAsync()
    {
        UseKestrel(0); // 0 = random free port
        using var client = CreateClient();
        BaseUrl = client.BaseAddress!.ToString().TrimEnd('/');

        return ValueTask.CompletedTask;
    }
}

[CollectionDefinition("Sandbox")]
public class SandboxCollection : ICollectionFixture<SandboxServerFixture>;