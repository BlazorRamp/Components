using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sandbox.Contracts.Models;


public record class ContactMethodDto
{
    public string MethodType  { get; set; } = default!;
    public string MethodValue { get; set; } = default!;
}

public record class AddressDto
{
    public string AddressLine       { get; set; } = default!;
    public string TownCity          { get; set; } = default!;
    public string County            { get; set; } = default!;
    public string? NullablePostcode { get; set; } = default;
}
public record class ContactDto
{
    public string   Title          { get; set; } = default!;

    [Required(ErrorMessage = "Name is required")]
    [MinLength(4, ErrorMessage = "Name must be at least 4 characters")]
    public string   GivenName      { get; set; } = default!;
    [MinLength(4, ErrorMessage = "Name must be at least 4 characters")]
    public string   FamilyName     { get; set; } = default!;
    public DateOnly DOB            { get; set; } = default!;
    public DateOnly CompareDOB     { get; set; } = default!;
    public string   Email          { get; set; } = default!;
    public string?  NullableMobile { get; set; } 
    public int?     NullableAge    { get; set; }
    public int      Age            { get; set; }

    public List<string> Entries { get; set; } = [];

    public AddressDto  Address         { get; set; } = default!;
    public AddressDto? NullableAddress { get; set; }

    public List<ContactMethodDto> ContactMethods { get; set; } = [];
}
