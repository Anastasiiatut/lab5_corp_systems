using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab5_BLAZOR.Data;
using Lab5_BLAZOR.Models;
using Lab5_BLAZOR.Schemas;
namespace Lab5_BLAZOR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    [HttpGet("ActiveLoans")]
    public ActionResult<List<ActiveLoanSchema>> ActiveLoans()
    {
        using var db = new LibraryContext();

        List<ActiveLoanSchema> loans = db.BookLoans
            .Where(l => l.ReturnDate == null)
            .Select(l => new ActiveLoanSchema
            {
                LoanID = l.LoanID,
                ReaderName = l.Reader.LastName + " " + l.Reader.FirstName,
                Title = l.Book.Title,
                Author = l.Book.Author,
                BorrowDate = l.BorrowDate
            })
            .ToList();

        return Ok(loans);
    }

    [HttpGet("Books")]
    public ActionResult<List<BookSchema>> Books()
    {
        using var db = new LibraryContext();

        List<BookSchema> books = db.Books
                .Select(b => new BookSchema
                {
                    BookID = b.BookID,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price
                })
                .ToList();
        return Ok(books);
    }
    [HttpGet("AllLoans")]
    public ActionResult<List<BookLoanSchema>> AllLoans()
    {
        using var db = new LibraryContext();

        List<BookLoanSchema> loans = db.BookLoans
            .OrderByDescending(l => l.LoanID)
            .Select(l => new BookLoanSchema
            {
                LoanID = l.LoanID,
                ReaderName = l.Reader.LastName + " " + l.Reader.FirstName,
                Title = l.Book.Title,
                BorrowDate = l.BorrowDate,
                ReturnDate = l.ReturnDate
            })
            .ToList();
        return Ok(loans);
    }
    [HttpGet("Readers")]
    public ActionResult<List<ReaderSchema>> Readers()
    {
        using var db = new LibraryContext();

        List<ReaderSchema> readers = db.Readers
            .Select(r => new ReaderSchema{
                ReaderID = r.ReaderID,
                LastName = r.LastName,
                MiddleName = r.MiddleName,
                FirstName = r.FirstName,
                HomeAddress = r.HomeAddress,
                PhoneNumber = r.PhoneNumber,
                DateOfBirth = r.DateOfBirth,
                LibraryCardNumber = r.LibraryCardNumber
            }).ToList();
        return Ok(readers);
    }

    [HttpGet("ReturnBook")]
    public ActionResult<BookLoanSchema> ReturnBook(int id)
    {
        using var db = new LibraryContext();

        var loan = db.BookLoans
            .Include(l => l.Reader)
            .Include(l => l.Book)
            .FirstOrDefault(l => l.LoanID == id);

        if (loan == null) return NotFound();

        if (loan.ReturnDate == null)
        {
            loan.ReturnDate = DateTime.Now.ToString("yyyy-MM-dd");
            db.SaveChanges();
        }

        return Ok(new BookLoanSchema
        {
            LoanID = loan.LoanID,
            ReaderName = loan.Reader.LastName + " " + loan.Reader.FirstName,
            Title = loan.Book.Title,
            BorrowDate = loan.BorrowDate,
            ReturnDate = loan.ReturnDate
        });
    }

    public record AddLoanRequest(int ReaderID, int BookID);

    [HttpPost("AddLoan")]
    public ActionResult<BookLoanSchema> AddLoan([FromBody] AddLoanRequest req)
    {
        using var db = new LibraryContext();

        var loan = new BookLoan
        {
            ReaderID = req.ReaderID,
            BookID = req.BookID,
            BorrowDate = DateTime.Now.ToString("yyyy-MM-dd"),
            ReturnDate = null
        };

        db.BookLoans.Add(loan);
        db.SaveChanges();

        var saved = db.BookLoans
            .Include(l => l.Reader)
            .Include(l => l.Book)
            .First(l => l.LoanID == loan.LoanID);

        return Ok(new BookLoanSchema
        {
            LoanID = saved.LoanID,
            ReaderName = saved.Reader.LastName + " " + saved.Reader.FirstName,
            Title = saved.Book.Title,
            BorrowDate = saved.BorrowDate,
            ReturnDate = saved.ReturnDate
        });
    }

    public record CreateBookRequest(string Title, string Author, double Price);

    [HttpPost("CreateBook")]
    public ActionResult<BookSchema> CreateBook([FromBody] CreateBookRequest req)
    {
        using var db = new LibraryContext();

        var book = new Book
        {
            Title = req.Title,
            Author = req.Author,
            Price = req.Price
        };
        db.Books.Add(book);
        db.SaveChanges();

        return Ok(new BookSchema
        {
            BookID = book.BookID,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price
        });
    }

    public record CreateReaderRequest(
        string LastName,
        string FirstName,
        string? MiddleName,
        string? Address,
        string? Phone,
        string? Dob,
        string Card
    );

    [HttpPost("CreateReader")]
    public ActionResult<ReaderSchema> CreateReader([FromBody] CreateReaderRequest req)
    {
        using var db = new LibraryContext();

        var reader = new Reader
        {
            LastName = req.LastName,
            FirstName = req.FirstName,
            MiddleName = req.MiddleName,
            HomeAddress = req.Address,
            PhoneNumber = req.Phone,
            DateOfBirth = req.Dob,
            LibraryCardNumber = req.Card
        };

        db.Readers.Add(reader);
        db.SaveChanges();

        return Ok(new ReaderSchema
        {
            ReaderID = reader.ReaderID,
            LastName = reader.LastName,
            FirstName = reader.FirstName,
            MiddleName = reader.MiddleName,
            HomeAddress = reader.HomeAddress,
            PhoneNumber = reader.PhoneNumber,
            DateOfBirth = reader.DateOfBirth,
            LibraryCardNumber = reader.LibraryCardNumber
        });
    }
}
