using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Enumerations;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces.BookRequests;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessTests;

public class BookRequestsRepositoryTests
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

    private static readonly BookEntity book2 = new()
    {
        BookId = Guid.Parse("00c0ea59-15cb-4a19-4444-4c4313833aef"),
        Isbn = "0987654321",
        Title = "Harry Potter",
        Description = "Harry Potter Book",
        Genre = "Fantasy",
        CoverUrl = "http://coverurl",
        PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2")
    };

    private IBookRequestsRepository _bookRequestsRepository = null!;
    private DataContext _context = null!;
    private ILogger<BookRequestsRepository> _logger = null!;
    private BookRequestEntity bookRequestEntity1;
    private BookRequestEntity bookRequestEntity2;

    private readonly PagingParameters pagingParameters = new()
    {
        PageNumber = 1,
        PageSize = 10
    };


    [SetUp]
    public async Task Setup()
    {
        _logger = Substitute.For<ILogger<BookRequestsRepository>>();
        _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
        if (_context != null) _bookRequestsRepository = new BookRequestsRepository(_context, _logger);
        await RoleSeeder.SeedRolesAsync(_context);
        await UserSeeder.SeedUsersAsync(_context);
        bookRequestEntity1 = new BookRequestEntity
        {
            BookRequestId = new Guid(),
            Status = BookRequestStatus.Pending,
            CreatedAt = DateTime.Now,
            Customer = _context.Customers.Include(c => c.User).First(),
            BookItem = new BookItemEntity
            {
                Book = new BookEntity
                {
                    Publisher = new PublisherEntity()
                }
            }
        };
        bookRequestEntity2 = new BookRequestEntity
        {
            BookRequestId = new Guid(),
            Status = BookRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Customer = _context.Customers.Include(c => c.User).First(),
            BookItem = new BookItemEntity
            {
                Book = new BookEntity
                {
                    Publisher = new PublisherEntity()
                }
            }
        };
        await _context.Books.AddAsync(book1);
        await _context.Books.AddAsync(book2);
        await _context.BookItems.AddAsync(new BookItemEntity
        {
            Barcode = "43546fsd6345",
            BookStatus = BookItemStatusEnumeration.Available,
            BookId = book1.BookId
        });
        await _context.BookRequests.AddAsync(bookRequestEntity1);
        await _context.BookRequests.AddAsync(bookRequestEntity2);
        await _context.SaveChangesAsync();
    }

    [Test]
    public void GetAllRequests()
    {
        var bookRequests = _bookRequestsRepository.GetAllRequests(pagingParameters);
        Assert.That(bookRequests.Data, Is.Not.Null.Or.Empty);
        Assert.That(bookRequests.Data.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetUserRequests()
    {
        var customer = await _context.Customers.Include(c => c.User).FirstAsync();
        var bookRequests = await _bookRequestsRepository.GetUserRequests(customer.CustomerId, pagingParameters);
        Assert.That(bookRequests.Data, Is.Not.Null.Or.Empty);
        Assert.That(bookRequests.Data.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetUserRequestsFails()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async delegate
        {
            await _bookRequestsRepository.GetUserRequests(new Guid(), pagingParameters);
        });
    }

    [Test]
    public async Task MakeRequest()
    {
        var bookRequestModel = new BookRequestModel
        {
            BookId = book1.BookId,
            RequestedBy = (await _context.Customers.Include(c => c.User).FirstAsync()).CustomerId
        };
        var createdRequest = await _bookRequestsRepository.MakeRequest(bookRequestModel);
        Assert.That(createdRequest, Is.Not.Null.Or.Empty);
    }

    [Test]
    public async Task MakeRequest_Fails_BookDoesNotExist()
    {
        var bookRequestModel = new BookRequestModel
        {
            BookId = new Guid(),
            RequestedBy = (await _context.Customers.Include(c => c.User).FirstAsync()).CustomerId
        };
        Assert.ThrowsAsync<KeyNotFoundException>(async delegate
        {
            await _bookRequestsRepository.MakeRequest(bookRequestModel);
        });
    }

    [Test]
    public async Task MakeRequest_Fails_AlreadyRequestedBook()
    {
        var bookRequestModel = new BookRequestModel
        {
            BookId = book1.BookId,
            RequestedBy = (await _context.Customers.Include(c => c.User).FirstAsync()).CustomerId
        };
        var createdRequest = await _bookRequestsRepository.MakeRequest(bookRequestModel);
        Assert.ThrowsAsync<AppException>(async delegate
        {
            await _bookRequestsRepository.MakeRequest(bookRequestModel);
        });
    }

    [Test]
    public async Task MakeRequest_Fails_NoCopiesAvailable()
    {
        var bookRequestModel = new BookRequestModel
        {
            BookId = book2.BookId,
            RequestedBy = (await _context.Customers.Include(c => c.User).FirstAsync()).CustomerId
        };
        Assert.ThrowsAsync<AppException>(async delegate
        {
            await _bookRequestsRepository.MakeRequest(bookRequestModel);
        });
    }
}