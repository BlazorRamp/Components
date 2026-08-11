namespace BlazorRamp.WebSite.common.Models;

public record class SomePersonData(string FirstName, string Surname, int Age, string Country);

public record SomePersonView(string FullName, string Spouse);


public class Contact(string surname, string email)
{
    public string Surname { get; set; } = surname;
    public string Email { get; set; }  = email;
}

public record class AnotherContact(int ContactID, string Title, string GivenName, string FamilyName, DateOnly DateOfBirth, string Country, decimal Rate, string Availability);