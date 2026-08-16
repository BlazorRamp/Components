namespace BlazorRamp.DocSite.Common.Models;

public record class SomePersonData(string FirstName, string Surname, int Age, string Country);

public record SomePersonView(string FullName, string Spouse);

public record class Contact(int ContactID, string Title, string GivenName, string FamilyName, DateOnly DateOfBirth, string Country, decimal Rate, string Availability);

public record RgbColour(byte Red = 0, byte Green = 0, byte Blue = 0);