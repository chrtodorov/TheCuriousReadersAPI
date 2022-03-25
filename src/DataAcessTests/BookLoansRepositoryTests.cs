using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookLoans;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DataAccessTests;

public class BookLoansRepositoryTests
{
    private static readonly BookEntity book1 = new()
    {
        BookId = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        Isbn = "1234567890",
        Title = "Harry Potter",
        Description = "Harry Potter Book",
        Genre = "Fantasy",
        CoverUrl = "http://coverurl",
        PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2")
    };

    private static readonly BookItemEntity bookItem1 = new()
    {
        BookItemId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2"),
        Barcode = "43546fsd6345",
        BookStatus = BookItemStatusEnumeration.Reserved,
        BookId = book1.BookId
    };

    private static readonly BookItemEntity invalidBookItem = new()
    {
        BookItemId = Guid.Parse("45634578-f62a-478b-a51b-11d2367136d2"),
        Barcode = "43546f473345",
        BookStatus = BookItemStatusEnumeration.Available,
        BookId = book1.BookId
    };

    private IBookLoansRepository _bookLoansRepository = null!;
    private DataContext _context = null!;
    private BookLoanEntity bookLoanEntity = null!;

    private readonly PagingParameters pagingParameters = new()
    {
        PageNumber = 1,
        PageSize = 10
    };

    [SetUp]
    public async Task Setup()
    {
        _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
        if (_context != null) _bookLoansRepository = new BookLoansRepository(_context);

        await RoleSeeder.SeedRolesAsync(_context);
        await UserSeeder.SeedUsersAsync(_context);
        bookLoanEntity = new BookLoanEntity
        {
            BookLoanId = new Guid(),
            Customer = _context.Customers.Include(c => c.User).First(),
            From = DateTime.Now,
            To = DateTime.Now.AddDays(10),
            TimesExtended = 0,
            Librarian = _context.Librarians.Include(c => c.User).First(),
            BookItem = new BookItemEntity
            {
                Book = new BookEntity
                {
                    Publisher = new PublisherEntity()
                }
            }
        };
        await _context.BookLoans.AddAsync(bookLoanEntity);

        await _context.BookLoans.AddAsync(new BookLoanEntity
        {
            BookLoanId = new Guid(),
            Customer = _context.Customers.Include(c => c.User).First(),
            From = DateTime.Now,
            To = DateTime.Now.AddDays(34),
            TimesExtended = 0,
            Librarian = _context.Librarians.Include(c => c.User).First(),
            BookItem = new BookItemEntity
            {
                Book = new BookEntity
                {
                    Publisher = new PublisherEntity()
                }
            }
        });

        await _context.Books.AddAsync(book1);
        await _context.BookItems.AddAsync(bookItem1);
        await _context.BookItems.AddAsync(invalidBookItem);
        await _context.SaveChangesAsync();
    }

    [Test]
    public void GetAll()
    {
        var loans = _bookLoansRepository.GetAll(pagingParameters);
        Assert.That(loans, Is.Not.Null.Or.Empty);
        Assert.That(loans.Data.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetExpiring()
    {
        var loans = _bookLoansRepository.GetExpiring(pagingParameters);
        Assert.That(loans, Is.Not.Null.Or.Empty);
        Assert.That(loans.Data.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetLoanById()
    {
        var userId = (await _context.Customers.Include(c => c.User).FirstAsync()).User.UserId;
        var loans = await _bookLoansRepository.GetLoansById(userId, pagingParameters);
        Assert.That(loans, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void GetLoanById_Fails_UserDoesNotExist()
    {
        var userId = new Guid();
        Assert.ThrowsAsync<KeyNotFoundException>(async delegate
        {
            await _bookLoansRepository.GetLoansById(userId, pagingParameters);
        });
    }

    [Test]
    public async Task LoanBook()
    {
        var bookRequestEntity1 = new BookRequestEntity
        {
            BookRequestId = new Guid(),
            Status = BookRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Customer = _context.Customers.Include(c => c.User).First(),
            BookItem = bookItem1
        };
        await _context.BookRequests.AddAsync(bookRequestEntity1);
        await _context.SaveChangesAsync();

        var bookLoan = new BookLoan
        {
            LoanedToId = (await _context.Customers.Include(c => c.User).FirstAsync()).CustomerId,
            BookItemId = bookItem1.BookItemId
        };

        var loan = await _bookLoansRepository.LoanBook(bookLoan);
        Assert.That(loan, Is.Not.Null.Or.Empty);
        await _context.DisposeAsync();
    }

    [Test]
    public void LoanBook_Fails_CustomerDoesNotExist()
    {
        var bookLoan = new BookLoan
        {
            LoanedToId = new Guid(),
            BookItemId = bookItem1.BookItemId
        };
        Assert.ThrowsAsync<KeyNotFoundException>(async delegate { await _bookLoansRepository.LoanBook(bookLoan); });
    }

    [Test]
    public async Task LoanBook_Fails_BookCopyDoesNotExist()
    {
        var bookLoan = new BookLoan
        {
            LoanedToId = (await _context.Customers.Include(c => c.User).FirstAsync()).CustomerId,
            BookItemId = new Guid()
        };
        Assert.ThrowsAsync<KeyNotFoundException>(async delegate { await _bookLoansRepository.LoanBook(bookLoan); });
    }

    [Test]
    public async Task LoanBook_Fails_InvalidStatus()
    {
        var bookLoan = new BookLoan
        {
            LoanedToId = new Guid(),
            BookItemId = invalidBookItem.BookItemId
        };
        Assert.ThrowsAsync<KeyNotFoundException>(async delegate { await _bookLoansRepository.LoanBook(bookLoan); });

        invalidBookItem.BookStatus = BookItemStatusEnumeration.Borrowed;
        _context.Update(invalidBookItem);
        await _context.SaveChangesAsync();

        Assert.ThrowsAsync<KeyNotFoundException>(async delegate { await _bookLoansRepository.LoanBook(bookLoan); });

        invalidBookItem.BookStatus = BookItemStatusEnumeration.NotAvailable;
        _context.Update(invalidBookItem);
        await _context.SaveChangesAsync();

        Assert.ThrowsAsync<KeyNotFoundException>(async delegate { await _bookLoansRepository.LoanBook(bookLoan); });
    }
}