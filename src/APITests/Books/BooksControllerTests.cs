using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using BusinessLayer.Interfaces.Books;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace APITests.Books;

public class BooksControllerTests
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
        }
    };

    private readonly BookRequest _bookRequestData = new()
    {
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
        }
    };

    private BooksController _booksController;
    private IBooksService _booksService;
    private ILogger<BooksController> _logger;

    [SetUp]
    public void Setup()
    {
        _booksService = Substitute.For<IBooksService>();
        _logger = Substitute.For<ILogger<BooksController>>();

        _booksController = new BooksController(_booksService, _logger)
        {
            ControllerContext = new ControllerContext()
        };
    }

    [Test]
    public async Task GetBooksAsync()
    {
        var books = new List<Book> {_bookData};
        var pagedList = new PagedList<Book>(books, 1, 1, 1);

        _booksService.GetBooks(Arg.Any<BookParameters>()).Returns(pagedList);

        var result = await _booksController.GetBooks(new BookParameters());

        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var pagedListResult = okResult?.Value as PagedList<Book>;

        Assert.IsNotNull(pagedListResult);
        Assert.AreEqual(pagedList, pagedListResult);
    }

    [Test]
    public async Task GetLatest()
    {
        var books = new List<Book> {_bookData};

        _booksService.GetLatest().Returns(books);

        var list = await _booksController.GetLatest();

        var okResult = list as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));

        var listResult = okResult?.Value as List<Book>;

        Assert.That(listResult, Is.Not.Null);
        Assert.That(listResult, Is.EqualTo(books));
    }

    [Test]
    public async Task CreateAsync_Ok()
    {
        _booksService.Create(Arg.Any<Book>()).Returns(_bookData);

        var result = await _booksController.Create(_bookRequestData);

        await _booksService.Received(1).Create(Arg.Is<Book>(a =>
            a.Title == _bookData.Title &&
            a.Isbn == _bookData.Isbn &&
            a.Genre == _bookData.Genre &&
            a.CoverUrl == _bookData.CoverUrl &&
            a.AuthorsIds!.SequenceEqual(_bookData.AuthorsIds!) &&
            a.Description == _bookData.Description &&
            a.PublisherId == _bookData.PublisherId));

        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var bookResult = okResult?.Value as Book;

        Assert.AreEqual(_bookData, bookResult);
    }

    [Test]
    public async Task CreateAsync_InvalidModel()
    {
        _booksController.ModelState.AddModelError("test", "test");

        var result = await _booksController.Create(new BookRequest());

        var invalidResult = result as ObjectResult;

        Assert.IsNotNull(invalidResult);
        Assert.AreEqual(400, invalidResult?.StatusCode);
    }

    [Test]
    public async Task UpdateAsync()
    {
        _booksService.Update(Arg.Any<Guid>(), Arg.Any<Book>()).Returns(_bookData);

        _booksService.Contains(Arg.Any<Guid>()).Returns(true);

        var result = await _booksController.Update(_bookData.BookId, _bookRequestData);

        await _booksService.Received(1).Update(Arg.Any<Guid>(), Arg.Is<Book>(a =>
            a.Title == _bookData.Title &&
            a.Isbn == _bookData.Isbn &&
            a.Genre == _bookData.Genre &&
            a.CoverUrl == _bookData.CoverUrl &&
            a.AuthorsIds!.SequenceEqual(_bookData.AuthorsIds!) &&
            a.Description == _bookData.Description &&
            a.PublisherId == _bookData.PublisherId));

        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var bookResult = okResult?.Value as Book;

        Assert.AreEqual(_bookData, bookResult);
    }

    [Test]
    public async Task UpdateAsync_InvalidModel()
    {
        _booksController.ModelState.AddModelError("test", "test");

        var result = await _booksController.Update(new Guid(), new BookRequest());

        var invalidResult = result as ObjectResult;

        Assert.IsNotNull(invalidResult);
        Assert.AreEqual(400, invalidResult?.StatusCode);
    }

    [Test]
    public async Task DeleteAsync()
    {
        await _booksService.Delete(Arg.Any<Guid>());

        _booksService.Contains(Arg.Any<Guid>()).Returns(true);

        var result = await _booksController.Delete(_bookData.BookId);
        await _booksService.Received(1).Delete(Arg.Any<Guid>());

        Assert.IsNotNull(result);
    }

    [Test]
    public async Task MakeUnavailable()
    {
        await _booksService.MakeUnavailable(Arg.Any<Guid>());

        var result = await _booksController.MakeUnavailable(_bookData.BookId);
        await _booksService.Received(1).MakeUnavailable(Arg.Any<Guid>());

        Assert.IsNotNull(result);
    }

    [Test]
    public async Task MakeUnavailable_NotFound()
    {
        var fakeId = new Guid();
        await _booksService.MakeUnavailable(Arg.Any<Guid>());

        var result = await _booksController.MakeUnavailable(fakeId);
        await _booksService.Received(1).MakeUnavailable(Arg.Any<Guid>());

        var resultNotFound = result as NotFoundObjectResult;

        Assert.That(resultNotFound, Is.Null);
    }

    [Test]
    public async Task GetNumber()
    {
        var number = 1;
        _booksService.GetNumber().Returns(number);

        var result = await _booksController.GetNumber();
        await _booksService.Received(1).GetNumber();

        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var numberOfBooks = okResult?.Value as int?;

        Assert.IsNotNull(numberOfBooks);
        Assert.AreEqual(number, numberOfBooks);
    }
}