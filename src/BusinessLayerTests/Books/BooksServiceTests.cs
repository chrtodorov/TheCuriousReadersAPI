using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Responses;
using BusinessLayer.Services;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLayerTests.Books;

public class BooksServiceTests
{
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
            new()
            {
                Barcode = "123456",
                BookItemId = Guid.Parse("3b75f22e-6721-482b-a91f-7772a38fd105"),
                BookStatus = 0
            }
        }
    };

    private readonly BookDetailsResponse _bookDataResponse = new()
    {
        BookId = Guid.Parse("9fc0ae59-15cb-4a19-9916-4c431383fab5"),
        Isbn = "1234567890",
        Title = "Harry Potter",
        Description = "Harry Potter Book",
        Genre = "Fantasy",
        CoverUrl = "http://coverurl",
        Publisher = new Publisher {Name = "Me4o", PublisherId = Guid.Parse("399b3630-f62a-478b-a51b-11d2367136d2")},
        Authors = new List<Author>
            {new() {Name = "Author", AuthorId = Guid.Parse("53e49024-2062-41cd-a04a-7772a38fd105")}}
    };

    private IBlobService _blobService;
    private IBookItemsRepository _bookItemsRepository;
    private IBooksRepository _booksRepository;
    private IBooksService _booksService;


    [SetUp]
    public void Setup()
    {
        _booksRepository = Substitute.For<IBooksRepository>();
        _bookItemsRepository = Substitute.For<IBookItemsRepository>();
        _blobService = Substitute.For<IBlobService>();
        _booksService = new BooksService(_booksRepository, _bookItemsRepository, _blobService);
    }

    [Test]
    public async Task GetAsync_Success()
    {
        _booksRepository.Get(_bookDataResponse.BookId).Returns(_bookDataResponse);

        var book = await _booksService.Get(_bookData.BookId);

        Assert.AreEqual(_bookData.BookId, book?.BookId);
    }

    [Test]
    public async Task GetAsync_Fail()
    {
        _booksRepository.Get(_bookDataResponse.BookId).Returns(_bookDataResponse);

        Assert.ThrowsAsync<KeyNotFoundException>(async delegate { await _booksService.Get(new Guid()); });
    }

    [Test]
    public async Task GetCount()
    {
        var number = 1;

        _booksRepository.GetNumber().Returns(number);

        var book = await _booksService.GetNumber();

        await _booksRepository.Received(1).GetNumber();

        Assert.That(book, Is.Not.Null);
    }

    [Test]
    public async Task GetLatest()
    {
        var books = new List<Book> {_bookData};

        _booksRepository.GetLatest().Returns(books);

        var receivedList = await _booksService.GetLatest();

        Assert.That(books, Is.EqualTo(receivedList));
    }

    [Test]
    public async Task GetBooksAsync()
    {
        var books = new List<Book> {_bookData};
        var pagedList = new PagedList<Book>(books, 1, 1, 1);

        _booksRepository.GetBooks(Arg.Any<BookParameters>()).Returns(pagedList);

        var receivedPagedList = await _booksService.GetBooks(new BookParameters());

        Assert.AreEqual(pagedList, receivedPagedList);
    }

    [Test]
    public async Task CreateAsync()
    {
        _booksRepository.Create(_bookData).Returns(_bookData);

        var createdBook = await _booksService.Create(_bookData);

        await _booksRepository.Received(1).Create(_bookData);

        Assert.AreEqual(createdBook, _bookData);
    }

    [Test]
    public async Task UpdateAsync_Success()
    {
        _booksRepository.Contains(_bookData.BookId).Returns(true);
        _booksRepository.Update(_bookData.BookId, _bookData).Returns(_bookData);

        var bookUpdate = await _booksService.Update(_bookData.BookId, _bookData);

        await _booksRepository.Received(1).Update(_bookData.BookId, _bookData);

        Assert.AreEqual(bookUpdate, _bookData);
    }

    [Test]
    public async Task UpdateAsync_Fail()
    {
        _booksRepository.Contains(_bookData.BookId).Returns(true);
        _booksRepository.Update(_bookData.BookId, _bookData).Returns(_bookData);

        var bookUpdate = await _booksService.Update(_bookData.BookId, _bookData);

        await _booksRepository.Received(1).Update(_bookData.BookId, _bookData);

        Book fakeBook = new()
        {
            BookId = Guid.Parse("305ed33d-40f6-42a0-8a03-27f8e45249e2"),
            Isbn = "3265987852",
            Title = "Potter Harry",
            Description = "Fake Book",
            Genre = "Fake Genre",
            CoverUrl = "http://coverurl_fake",
            PublisherId = Guid.Parse("e6f2337e-92b4-406f-b1ee-8d9835c66bf5"),
            AuthorsIds = new List<Guid>
            {
                Guid.Parse("dbca97b1-a0a2-41d3-aebf-e06ea388d43b"),
                Guid.Parse("ca16daf2-c4c5-43d8-bfe8-595daeed229f")
            }
        };

        Assert.AreNotEqual(bookUpdate, fakeBook);
    }

    [Test]
    public async Task DeleteAsync()
    {
        _booksRepository.Get(_bookData.BookId).Returns(_bookDataResponse);

        await _booksService.Delete(_bookData.BookId);

        await _booksRepository.Received(1).Delete(_bookData.BookId);

        await _booksRepository.Received(1).Get(_bookData.BookId);
    }

    [Test]
    public async Task ContainsAsync()
    {
        _booksRepository.Contains(_bookData.BookId).Returns(true);

        var result = await _booksService.Contains(_bookData.BookId);

        await _booksRepository.Received(1).Contains(_bookData.BookId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task MakeUnavailable()
    {
        _booksRepository.Get(_bookData.BookId).Returns(_bookDataResponse);

        await _booksService.MakeUnavailable(_bookData.BookId);

        await _booksRepository.Received(1).MakeUnavailable(_bookData.BookId);

        await _booksRepository.Received(1).Get(_bookData.BookId);
    }
}