using System.ComponentModel.DataAnnotations;

namespace Lab5_BLAZOR.Schemas;

public record ReaderSchema
{
    [Required]
    public int ReaderID { get; init; }

    [Required]
    public required string LastName { get; init; }

    [Required]
    public required string FirstName { get; init; }

    public string? MiddleName { get; init; }
    public string? HomeAddress { get; init; }
    public string? PhoneNumber { get; init; }
    public string? DateOfBirth { get; init; }

    [Required]
    public required string LibraryCardNumber { get; init; }
}
