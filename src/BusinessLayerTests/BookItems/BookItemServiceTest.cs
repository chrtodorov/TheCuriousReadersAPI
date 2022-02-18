using BusinessLayer.Interfaces.BookItems;
using BusinessLayer.Models;
using BusinessLayer.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerTests.BookItems
{
    public class BookItemServiceTest
    {
        private IBookItemsService _bookItemsService;
        private IBookItemsRepository _bookItemsRepository;

        BookItem bookItemsData = new BookItem
        {
            BookItemId = Guid.Parse("4b156c4e-dada-43d9-828e-714ae132033b"),
            Barcode = "1234567",
            BorrowedDate = new DateTime(2015 - 06 - 12),
            ReturnDate = new DateTime(2015 - 07 - 12),
            BookStatus = BusinessLayer.Enumerations.BookItemStatusEnumeration.Available,
            BookId = Guid.Parse("399b5384-870e-4cc4-a690-ec466f63dd98")
        };

        [SetUp]
        public void Setup()
        {
            _bookItemsRepository = Substitute.For<IBookItemsRepository>();
            _bookItemsService = new BookItemsService(_bookItemsRepository);
        }

        [Test]
        public async Task GetAsync_Success()
        {
            _bookItemsRepository.Get(bookItemsData.BookItemId).Returns(bookItemsData);
            var bookItem = await _bookItemsService.Get(bookItemsData.BookItemId);
            Assert.AreEqual(bookItemsData.BookItemId, bookItem.BookItemId);
        }
        [Test]
        public async Task GetAsync_Fail()
        {

            _bookItemsRepository.Get(bookItemsData.BookItemId).Returns(bookItemsData);
            var fakeId = Guid.NewGuid();

            var bookItem = await _bookItemsService.Get(bookItemsData.BookItemId);

            Assert.AreNotEqual(fakeId, bookItem.BookItemId);

        }

        [Test]
        public async Task CreateAsync()
        {
            _bookItemsRepository.Create(bookItemsData).Returns(bookItemsData);
            var bookItemC = await _bookItemsService.Create(bookItemsData);
            await _bookItemsRepository.Received(1).Create(bookItemsData);

            Assert.AreEqual(bookItemC.Barcode,bookItemsData.Barcode);
            Assert.AreEqual(bookItemC.BorrowedDate, bookItemsData.BorrowedDate);
            Assert.AreEqual(bookItemC.ReturnDate, bookItemsData.ReturnDate);
            Assert.AreEqual(bookItemC.BookStatus, bookItemsData.BookStatus);
            Assert.AreEqual(bookItemC.BookId, bookItemsData.BookId);
        }


        [Test]
        public async Task UpdateAsync()
        {
            _bookItemsRepository.Update(bookItemsData.BookItemId, bookItemsData).Returns(bookItemsData);
            var bookItemU = await _bookItemsService.Update(bookItemsData.BookItemId, bookItemsData);
            await _bookItemsRepository.Received(1).Update(bookItemsData.BookItemId, bookItemsData);

            Assert.AreEqual(bookItemU.Barcode, bookItemsData.Barcode);
            Assert.AreEqual(bookItemU.BorrowedDate, bookItemsData.BorrowedDate);
            Assert.AreEqual(bookItemU.ReturnDate, bookItemsData.ReturnDate);
            Assert.AreEqual(bookItemU.BookStatus, bookItemsData.BookStatus);
            Assert.AreEqual(bookItemU.BookId, bookItemsData.BookId);

        }
        [Test]
        public async Task UpdateAsyncFail()
        {
            _bookItemsRepository.Update(bookItemsData.BookItemId, bookItemsData).Returns(bookItemsData);
            var bookItemU = await _bookItemsService.Update(bookItemsData.BookItemId, bookItemsData);
            await _bookItemsRepository.Received(1).Update(bookItemsData.BookItemId, bookItemsData);

            var fakeBookItemId = Guid.NewGuid();
            var fakeBarcode = "7654321";
            var fakeBorrowedDate = DateTime.UtcNow;
            var fakeReturnDate = DateTime.UtcNow;
            var fakeBookStatus = BusinessLayer.Enumerations.BookItemStatusEnumeration.Borrowed;
            var fakeBookId = Guid.NewGuid();

            Assert.AreNotEqual(bookItemU.BookItemId, fakeBookItemId);
            Assert.AreNotEqual(bookItemU.Barcode, fakeBarcode);
            Assert.AreNotEqual(bookItemU.BorrowedDate, fakeBorrowedDate);
            Assert.AreNotEqual(bookItemU.ReturnDate, fakeReturnDate);
            Assert.AreNotEqual(bookItemU.BookStatus, fakeBookStatus);
            Assert.AreNotEqual(bookItemU.BookId, fakeBookId);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var deleted = _bookItemsService.Delete(bookItemsData.BookItemId);
            await _bookItemsRepository.Received(1).Delete(bookItemsData.BookItemId);
        }

        [Test]
        public async Task Contains()
        {
            bool expectedRes = true;
            _bookItemsRepository.Contains(bookItemsData.BookItemId).Returns(expectedRes);
            var result = await _bookItemsService.Contains(bookItemsData.BookItemId);
            await _bookItemsRepository.Received(1).Contains(bookItemsData.BookItemId);
            Assert.That(result, Is.EqualTo(expectedRes));
        }
    }
}
