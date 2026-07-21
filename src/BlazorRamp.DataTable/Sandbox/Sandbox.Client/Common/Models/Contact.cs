namespace Sandbox.Client.Common.Models;

public record class Contact(int ContactID, string Title, string GivenName, string FamilyName, DateOnly DateOfBirth, string Country, decimal Rate);


