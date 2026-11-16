using System.ComponentModel.DataAnnotations;

namespace Lab5_BLAZOR.Schemas;

public record BookSchema
{
    [Required]
    public int BookID { get; init; }

    [Required]
    public required string Title { get; init; }

    [Required]
    public required string Author { get; init; }

    public double Price { get; init; }
}
