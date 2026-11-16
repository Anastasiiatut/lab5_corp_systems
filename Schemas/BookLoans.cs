using System.ComponentModel.DataAnnotations;

namespace Lab5_BLAZOR.Schemas;

public record BookLoanSchema
{
    [Required]
    public int LoanID { get; init; }

    [Required]
    public required string ReaderName { get; init; }

    [Required]
    public required string Title { get; init; }

    [Required]
    public required string BorrowDate { get; init; }

    public string? ReturnDate { get; init; }
}
