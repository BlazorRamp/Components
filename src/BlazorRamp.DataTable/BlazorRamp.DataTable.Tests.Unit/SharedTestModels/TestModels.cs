namespace BlazorRamp.DataTable.Tests.Unit.SharedTestModels;

public record class Contact(int ContactID, string Title, string GivenName, string FamilyName, DateOnly DateOfBirth, string Country, decimal Rate);