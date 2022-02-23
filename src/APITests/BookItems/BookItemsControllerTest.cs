using API.Controllers;
using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Models;
using BusinessLayer.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITests.BookItems
{
    public class BookItemsControllerTest
    {
        private IBookItemsService _bookItemsService;
        private ILogger<BookItemsController> _logger;

        private BookItemsController _bookItemsController;

        BookItem bookItemsData = new BookItem
        {
            BookItemId = Guid.NewGuid(),
            Barcode = "1234567",
            BorrowedDate = new DateTime(2015 - 06 - 12),
            ReturnDate = new DateTime(2015 - 07 - 12),
            BookStatus = BusinessLayer.Enumerations.BookItemStatusEnumeration.Available,
            BookId = Guid.Parse("4b156c4e-dada-43d9-828e-714ae132033b")
        };

        BookItemsRequest bookItemsRequest = new BookItemsRequest
        {
            Barcode = "1234567",
            BorrowedDate = new DateTime(2015 - 06 - 12),
            ReturnDate = new DateTime(2015 - 07 - 12),
            BookStatus = BusinessLayer.Enumerations.BookItemStatusEnumeration.Available,
            BookId = Guid.Parse("4b156c4e-dada-43d9-828e-714ae132033b")
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
            Assert.That(bookItemResult.BookId, Is.EqualTo(bookItemsData.BookId));
        }
        [Test]
        public async Task GetAsync_NotFound()
        {
            var fakeId = Guid.NewGuid();
            BookItem error = null;

            _bookItemsService.Get(fakeId).Returns(error);
            var result = await _bookItemsController.Get(fakeId);
            await _bookItemsService.Received(1).Get(fakeId);

            var okResult = result as ObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateAsync()
        {

            _bookItemsService.Create(Arg.Any<BookItem>()).Returns(bookItemsData);

            var resultC = await _bookItemsController.Create(bookItemsRequest);

            await _bookItemsService.Received(1).Create(Arg.Is<BookItem>(bi =>
            bi.Barcode == bookItemsData.Barcode &&
            bi.BorrowedDate == bookItemsData.BorrowedDate &&
            bi.ReturnDate == bookItemsData.ReturnDate &&
            bi.BookStatus == bookItemsData.BookStatus &&
            bi.BookId == bookItemsData.BookId
            ));

            var okResult = resultC as ObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var bookItemResult = okResult.Value as BookItem;

            Assert.IsNotNull(bookItemResult);

            Assert.That(bookItemResult.Barcode, Is.EqualTo(bookItemsData.Barcode));
            Assert.That(bookItemResult.BorrowedDate, Is.EqualTo(bookItemsData.BorrowedDate));
            Assert.That(bookItemResult.ReturnDate, Is.EqualTo(bookItemsData.ReturnDate));
            Assert.That(bookItemResult.BookStatus, Is.EqualTo(bookItemsData.BookStatus));
            Assert.That(bookItemResult.BookId, Is.EqualTo(bookItemsData.BookId));
        }
        [Test]
        public async Task UpdateAsync()
        {
            _bookItemsService.Update(Arg.Any<Guid>(), Arg.Any<BookItem>()).Returns(bookItemsData);

            _bookItemsService.Contains(Arg.Any<Guid>()).Returns(true);

            var resultU = await _bookItemsController.Update(bookItemsData.BookId!.Value, bookItemsRequest);

            await _bookItemsService.Received(1).Update(Arg.Any<Guid>(), Arg.Is<BookItem>(bi =>
                bi.Barcode == bookItemsData.Barcode &&
                bi.BorrowedDate == bookItemsData.BorrowedDate &&
                bi.ReturnDate == bookItemsData.ReturnDate &&
                bi.BookStatus == bookItemsData.BookStatus &&
                bi.BookId == bookItemsData.BookId
            ));

            var statusResult = resultU as ObjectResult;

            Assert.IsNotNull(statusResult);
            Assert.AreEqual(200, statusResult.StatusCode);

            var valueResult = statusResult.Value as BookItem;

            Assert.AreEqual(valueResult.Barcode, bookItemsData.Barcode);
        }
        [Test]
        public async Task UpdateAsync_NotFound()
        {
            _bookItemsService.Update(Arg.Any<Guid>(), Arg.Any<BookItem>()).Returns(bookItemsData);

            _bookItemsService.Contains(Arg.Any<Guid>()).Returns(false);

            var resultU = await _bookItemsController.Update(bookItemsData.BookItemId, bookItemsRequest);

            await _bookItemsService.DidNotReceive().Update(Arg.Any<Guid>(), Arg.Any<BookItem>());

            var statusResult = resultU as ObjectResult;

            Assert.IsNotNull(statusResult);
            Assert.AreEqual(404, statusResult.StatusCode);
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
        [Test]
        public async Task DeleteAsync_NotFound()
        {
            await _bookItemsService.Delete(Arg.Any<Guid>());
            _bookItemsService.Contains(Arg.Any<Guid>()).Returns(false);

            var resultD = await _bookItemsController.Delete(bookItemsData.BookItemId);
            await _bookItemsService.DidNotReceive().Delete(Arg.Any<Guid>());

            Assert.That(resultD, Is.Not.Null);
        }
    }
}
