namespace Sandbox.Client.Common.Models;

public record class SomePersonData(string FirstName, string Surname, int Age, string Country);

public record SomePersonView(string FullName, string Spouse);
