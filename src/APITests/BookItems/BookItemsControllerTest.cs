using System;
using System.Threading.Tasks;
using API.Controllers;
using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace APITests.BookItems;

public class BookItemsControllerTest
{
    private readonly BookItem bookItemsData = new()
    {
        BookItemId = Guid.NewGuid(),
        Barcode = "1234567",
        BorrowedDate = new DateTime(2015 - 06 - 12),
        ReturnDate = new DateTime(2015 - 07 - 12),
        BookStatus = BookItemStatusEnumeration.Available
    };

    private BookItemsController _bookItemsController;
    private IBookItemsService _bookItemsService;
    private ILogger<BookItemsController> _logger;

    private BookItemsRequest bookItemsRequest = new()
    {
        Barcode = "1234567"
    };

    [SetUp]
    public void Setup()
    {
        _bookItemsService = Substitute.For<IBookItemsService>();
        _logger = Substitute.For<ILogger<BookItemsController>>();

        _bookItemsController = new BookItemsController(_bookItemsService, _logger);
    }

    [Test]
    public async Task GetAsync_Ok()
    {
        _bookItemsService.Get(Arg.Any<Guid>()).Returns(bookItemsData);
        var result = await _bookItemsController.Get(bookItemsData.BookItemId);
        await _bookItemsService.Received(1).Get(Arg.Any<Guid>());

        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var bookItemResult = okResult.Value as BookItem;

        Assert.IsNotNull(bookItemResult);
        Assert.That(bookItemResult.BookItemId, Is.EqualTo(bookItemsData.BookItemId));
        Assert.That(bookItemResult.Barcode, Is.EqualTo(bookItemsData.Barcode));
        Assert.That(bookItemResult.BorrowedDate, Is.EqualTo(bookItemsData.BorrowedDate));
        Assert.That(bookItemResult.ReturnDate, Is.EqualTo(bookItemsData.ReturnDate));
        Assert.That(bookItemResult.BookStatus, Is.EqualTo(bookItemsData.BookStatus));
    }

    [Test]
    public async Task DeleteAsync()
    {
        await _bookItemsService.Delete(Arg.Any<Guid>());
        _bookItemsService.Contains(Arg.Any<Guid>()).Returns(true);

        var resultD = await _bookItemsController.Delete(bookItemsData.BookItemId);
        await _bookItemsService.Received(1).Delete(Arg.Any<Guid>());

        Assert.That(resultD, Is.Not.Null);
    }
}