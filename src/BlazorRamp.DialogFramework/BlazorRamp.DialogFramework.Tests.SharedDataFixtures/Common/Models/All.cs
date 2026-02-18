namespace BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;

public record class SomePersonData(string FirstName, string Surname, int Age, string Country);

public record SomePersonView(string FullName, string Spouse);


public class FakeDialogComponent
{
    public string Title { get; set; } = string.Empty;
    public int Count { get; set; } = 0;
}