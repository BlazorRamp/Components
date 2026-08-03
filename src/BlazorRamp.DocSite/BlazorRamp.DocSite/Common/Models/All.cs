namespace BlazorRamp.DocSite.Common.Models;

public record class SomePersonData(string FirstName, string Surname, int Age, string Country);

public record SomePersonView(string FullName, string Spouse);

public record class Contact(int ContactID, string Title, string GivenName, string FamilyName, DateOnly DateOfBirth, string Country, decimal Rate);