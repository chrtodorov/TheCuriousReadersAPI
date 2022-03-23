using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Responses;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Mappers;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessTests;

public class BookRepositoryTests
{
    private  IBooksRepository _booksRepository;
    private  ILogger<BooksRepository> _logger;
    private  DataContext _context;

    private readonly Book _bookData = new()
    {
        BookId = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        Isbn = "1234567890",
        Title = "Harry Potter",
        Description = "Harry Potter Book",
        Genre = "Fantasy",
        CoverUrl = "http://coverurl",
        PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2"),
        AuthorsIds = new List<Guid>
        {
            Guid.Parse("53e49024-2062-41cd-a04a-7772a38fd105"),
            Guid.Parse("3b75f22e-6721-482b-a91f-f6c473d330e2")
        },
        BookItems = new List<BookItem>
        {
            new BookItem()
            {
                BookItemId = Guid.Parse("53e4a024-2042-41cd-a04a-7772a38fd405"),
                Barcode = "9765548372",
                BookStatus = 0,
                
            }
        }
    };

    private readonly BookEntity _bookDataEntity = new()
    {
        BookId = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        Isbn = "1234567890",
        Title = "Harry Potter",
        Description = "Harry Potter Book",
        Genre = "Fantasy",
        CoverUrl = "http://coverurl",
        Publisher = new PublisherEntity(){ PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2"), Name = "Bloomsbury Publishing" },
        Authors = new List<AuthorEntity>
        {
            new AuthorEntity(){AuthorId = Guid.Parse("53e49024-2062-41cd-a04a-7772a38fd105"), Name = "J. K. Rowling"},
            new AuthorEntity(){AuthorId = Guid.Parse("3b75f22e-6721-482b-a91f-f6c473d330e2"), Name = "JK Rowling"}
        },
        BookItems = new List<BookItemEntity>
        {
            new BookItemEntity(){
                BookItemId = Guid.Parse("53e49024-2062-41cd-a04a-7772a38fd195"), 
                Barcode = "9245185300", 
                BookStatus = 0,
                
            }
        }
    };
    private readonly BookEntity _bookDataEntity1 = new()
    {
        BookId = Guid.Parse("9fc0ae59-15cb-4b19-9916-4c431483fab5"),
        Isbn = "1234567891",
        Title = "Harry",
        Description = "Harry Potter Book",
        Genre = "Fantasy",
        CoverUrl = "http://coverurl",
        Publisher = new PublisherEntity() { PublisherId = Guid.Parse("399b3630-f62a-478b-651b-11d2367136d2"), Name = "Bloomsbur Publishing" },
        Authors = new List<AuthorEntity>
        {
            new AuthorEntity(){AuthorId = Guid.Parse("53e45024-2062-41cd-a04a-7772a38fd104"), Name = "J. K. Rowling"},
            new AuthorEntity(){AuthorId = Guid.Parse("3b75f22e-6721-482b-a21f-f6c473d330e2"), Name = "JK Rowling"}
        },
        BookItems = new List<BookItemEntity>
        {
            new BookItemEntity(){
                BookItemId = Guid.Parse("53e45124-2062-41cd-a04a-7772a38fd195"),
                Barcode = "9245185300",
                BookStatus = 0,

            }
        }
    };

    private readonly BookEntity _fakeBook = new()
    {
        BookId = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        PublisherId = Guid.NewGuid()
    };


    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For <ILogger<BooksRepository>>();
        _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();

        if (_context is not null)
        {
            _booksRepository = new BooksRepository(_context, _logger);
        }
    }

    [Test]
    public async Task GetAsync_ReturnsBook()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var testBookEntity = _context.Books.FirstOrDefault();

        var resultGetBook = await _booksRepository.Get(testBookEntity!.BookId);

        var testBook = _context.Books.FirstOrDefault()!.ToBookDetailsResponse();

        Assert.IsNotNull(resultGetBook);
        
        CollectionAssert.AreEqual(testBook.Authors?.Select(a => a.AuthorId), resultGetBook!.Authors?.Select(a => a.AuthorId));
        Assert.AreEqual(testBook.Title, resultGetBook.Title);
        Assert.AreEqual(testBook.Description, resultGetBook.Description);
        Assert.AreEqual(testBook.Publisher?.PublisherId, testBook.Publisher?.PublisherId);
        Assert.AreEqual(testBook.BookId, resultGetBook.BookId);
        Assert.AreEqual(testBook.CoverUrl, resultGetBook.CoverUrl);
        Assert.AreEqual(testBook.Genre, resultGetBook.Genre);
        Assert.AreEqual(testBook.Isbn, resultGetBook.Isbn);
    }
    [Test]
    public async Task GetAsync_ReturnsNull()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();
        
        var resultBook = await _booksRepository.Get(new Guid());

        Assert.Null(resultBook);
    }

    [Test]
    public async Task GetBooksAsync_WithoutFilter()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters());

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }
    [Test]
    public async Task GetBooksAsync_WithTittle()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters() {Title = "Harry Potter"});

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }
    [Test]
    public async Task GetBooksAsync_WithGenre()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters() { Genre = "Fantasy" });

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }
    [Test]
    public async Task GetBooksAsync_WithDescriptionKeyword()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters() { DescriptionKeyword = "Harry Potter Book" });

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }
    [Test]
    public async Task GetBooksAsync_WithAuthor()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters() { Author = "J. K. Rowling" });

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }
    [Test]
    public async Task GetBooksAsync_WithPublisher()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters() { Publisher = "Bloomsbury Publishing" });

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }
    [Test]
    public async Task GetBooksAsync_WithAllFilters()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var result = await _booksRepository.GetBooks(new BookParameters()
        {
            Publisher = "Bloomsbury Publishing", 
            Author = "J. K. Rowling", 
            Title = "Harry Potter", 
            Genre = "Fantasy", 
            DescriptionKeyword = "Harry Potter Book"
        });

        var test = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual(test.AuthorsIds, result.Data[0].AuthorsIds);
        Assert.AreEqual(test.Title, result.Data[0].Title);
        Assert.AreEqual(test.Description, result.Data[0].Description);
        Assert.AreEqual(test.PublisherId, result.Data[0].PublisherId);
        Assert.AreEqual(test.BookId, result.Data[0].BookId);
        Assert.AreEqual(test.CoverUrl, result.Data[0].CoverUrl);
        Assert.AreEqual(test.Genre, result.Data[0].Genre);
        Assert.AreEqual(test.Isbn, result.Data[0].Isbn);
    }

    [Test]
    public async Task GetLatestAsync()
    {
        _context.Books.Add(_bookDataEntity);
        _context.Books.Add(_bookDataEntity1);
        await _context.SaveChangesAsync();

        var booksToReceive = await _booksRepository.GetLatest();

        var books = _context.Books.FirstOrDefault()!.ToBook();

        Assert.IsNotNull(booksToReceive);
        Assert.That(books.Isbn, Is.EqualTo(booksToReceive[1].Isbn));
        Assert.That(books.Title, Is.EqualTo(booksToReceive[1].Title));
        Assert.That(books.Description, Is.EqualTo(booksToReceive[1].Description));
        Assert.That(books.PublisherId, Is.EqualTo(booksToReceive[1].PublisherId));
        Assert.That(books.BookId, Is.EqualTo(booksToReceive[1].BookId));
        Assert.That(books.Genre, Is.EqualTo(booksToReceive[1].Genre));

        CollectionAssert.AreEqual(books.AuthorsIds, booksToReceive[1].AuthorsIds);

        Assert.That(booksToReceive[0].CreatedAt, Is.GreaterThan(booksToReceive[1].CreatedAt));

    }

    [Test]
    public async Task CreateAsync()
    {
        var resultCreateBook = await _booksRepository.Create(_bookData);

        var testCreatedBookEntity = _context.Books.FirstOrDefault();

        Assert.IsNotNull(resultCreateBook);

        Assert.AreEqual(resultCreateBook.BookId, testCreatedBookEntity!.BookId);
    }

    [Test]
    public async Task UpdateAsync_Success()
    {
        _context.Books.Add(_fakeBook);

        await _context.SaveChangesAsync();

        var testReceivedEntity = _context.Books.FirstOrDefault();

        var resultUpdate = await _booksRepository.Update(testReceivedEntity!.BookId, _bookData);
        
        Assert.IsNotNull(resultUpdate);

        CollectionAssert.AreEqual(_bookData.AuthorsIds, resultUpdate!.AuthorsIds);
        Assert.AreEqual(_bookData.Title, resultUpdate.Title);
        Assert.AreEqual(_bookData.Description, resultUpdate.Description);
        Assert.AreEqual(_bookData.PublisherId, resultUpdate.PublisherId);
        Assert.AreEqual(_bookData.BookId, resultUpdate.BookId);
        Assert.AreEqual(_bookData.CoverUrl, resultUpdate.CoverUrl);
        Assert.AreEqual(_bookData.Genre, resultUpdate.Genre);
        Assert.AreEqual(_bookData.Isbn, resultUpdate.Isbn);
    }
    [Test]
    public async Task UpdateAsync_NotFound()
    {
        _context.Books.Add(_fakeBook);

        await _context.SaveChangesAsync();

        var resultUpdate = await _booksRepository.Update(new Guid(), _bookData);

        Assert.IsNull(resultUpdate);
    }

    [Test]
    public async Task DeleteAsync()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var testBookEntity = _context.Books.FirstOrDefault();

        await _booksRepository.Delete(testBookEntity!.BookId);

        var testDeleteBookEntity = _context.Books.FirstOrDefault();

        Assert.IsNull(testDeleteBookEntity);
    }

    [Test]
    public async Task ContainsAsync_True()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var testBookEntity = _context.Books.FirstOrDefault();

        var result = await _booksRepository.Contains(testBookEntity!.BookId);

        Assert.IsTrue(result);
    }
    [Test]
    public async Task ContainsAsync_False()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();
        
        var result = await _booksRepository.Contains(new Guid());

        Assert.IsFalse(result);
    }

    [Test]
    public async Task IsIsbnExisting()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var testBookEntity = _context.Books.FirstOrDefault();

        var result = await _booksRepository.IsIsbnExisting(testBookEntity!.Isbn);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task MakeUnavailableAsync()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var bookToChange = _context.Books.FirstOrDefault();
        var copyToChange = bookToChange.BookItems.FirstOrDefault();

        await _booksRepository.MakeUnavailable(bookToChange!.BookId);

        var copyChanged = bookToChange.BookItems.FirstOrDefault();

        Assert.That(copyChanged.BookStatus.ToString(), Is.EqualTo("NotAvailable"));
    }


    [Test]
    public async Task GetNumber()
    {
        _context.Books.Add(_bookDataEntity);
        await _context.SaveChangesAsync();

        var number = await _booksRepository.GetNumber();
        var defNumber = _context.Books.Count();

        Assert.That(defNumber, Is.EqualTo(number));
    }
}