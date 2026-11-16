using System.ComponentModel.DataAnnotations;

namespace Lab5_BLAZOR.Schemas;

public record ActiveLoanSchema
{
    [Required]
    public required int LoanID { get; init; }

    [Required]
    public required string ReaderName { get; init; }

    [Required]
    public required string Title { get; init; }

    [Required]
    public required string Author { get; init; }

    [Required]
    public required string BorrowDate { get; init; }
}
